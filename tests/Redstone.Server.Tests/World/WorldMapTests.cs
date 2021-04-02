using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Server.Registry;
using Redstone.Server.Tests.Mocks;
using Redstone.Server.World;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Redstone.Server.Tests.World
{
    /// <summary>
    /// Tests the <see cref="WorldMap"/> mechanism.
    /// </summary>
    public class WorldMapTests
    {
        private static readonly string _mapName = "minecraft:world";
        private readonly IRegistry _registry;
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<IBlockFactory> _blockFactoryMock;

        public WorldMapTests()
        {
            _blockFactoryMock = new Mock<IBlockFactory>();
            _registry = new DataRegistry(new Mock<ILogger<DataRegistry>>().Object);
            _serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger<WorldMap>>(new Mock<ILogger<WorldMap>>().Object)
                .AddSingleton(s => _blockFactoryMock.Object)
                .BuildServiceProvider();
        }

        [Fact]
        public void CreateWorldMapTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            Assert.NotNull(map);
            Assert.Equal(_mapName, map.Name);
            Assert.Empty(map.Regions);
            Assert.Empty(map.Players);
        }

        [Fact]
        public void CreateWorldWithoutNameTest()
        {
            Assert.Throws<ArgumentException>(() => new WorldMap(null, _serviceProvider));
        }

        [Fact]
        public void CreateWorldWithEmptyNameTest()
        {
            Assert.Throws<ArgumentException>(() => new WorldMap("", _serviceProvider));
        }

        [Fact]
        public void CreateWorldWithSpacesNameTest()
        {
            Assert.Throws<ArgumentException>(() => new WorldMap("     ", _serviceProvider));
        }

        [Fact]
        public void CreateWorldWithoutServiceProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new WorldMap("minecraft:world", null));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(-1, -1)]
        public void WorldMapAddRegionTest(int regionX, int regionZ)
        {
            var map = new WorldMap(_mapName, _serviceProvider);
            IRegion region = map.AddRegion(regionX, regionZ);

            Assert.NotNull(region);
            Assert.IsType<Region>(region);
            Assert.NotEmpty(map.Regions);
            Assert.Single(map.Regions);
            Assert.Equal(regionX, region.X);
            Assert.Equal(regionZ, region.Z);
            Assert.NotEmpty(region.Chunks);
            Assert.Equal(Math.Pow(Region.ChunkAmount, 2), region.Chunks.Count());
        }

        [Fact]
        public void WorldMapAddExistingRegionTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);
            IRegion region = map.AddRegion(0, 0);

            Assert.Throws<InvalidOperationException>(() => map.AddRegion(0, 0));
        }

        [Fact]
        public void WorldMapCheckIfContainsRegionTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);
            map.AddRegion(0, 0);

            Assert.True(map.ContainsRegion(0, 0));
        }

        [Fact]
        public void WorldMapCheckIfDoesntContainRegionTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            Assert.False(map.ContainsRegion(0, 0));
        }

        [Fact]
        public void WorldMapGetRegionTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);
            IRegion region = map.AddRegion(0, 0);
            IRegion mapRegion = map.GetRegion(0, 0);

            Assert.Same(region, mapRegion);
        }

        [Fact]
        public void WorldMapGetUnknownRegionTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            Assert.Null(map.GetRegion(0, 0));
        }

        [Fact]
        public void WorldMapAddPlayerTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayer(Guid.NewGuid());
            var map = new WorldMap(_mapName, _serviceProvider);

            map.AddPlayer(player);

            Assert.NotEmpty(map.Players);
            Assert.Single(map.Players);
            Assert.Same(map, player.Map);
        }

        [Fact]
        public void WorldMapAddExistingPlayerTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayer(Guid.NewGuid());
            var map = new WorldMap(_mapName, _serviceProvider);

            map.AddPlayer(player);

            Assert.NotEmpty(map.Players);
            Assert.Single(map.Players);
            Assert.Throws<InvalidOperationException>(() => map.AddPlayer(player));
        }

        [Fact]
        public void WorldMapRemovePlayerTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayer(Guid.NewGuid());
            var map = new WorldMap(_mapName, _serviceProvider);

            map.AddPlayer(player);
            var removedPlayer = map.RemovePlayer(player);

            Assert.NotNull(removedPlayer);
            Assert.Same(player, removedPlayer);
            Assert.Empty(map.Players);
        }

        [Fact]
        public void WorldMapRemoveUnknownPlayerTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayer(Guid.NewGuid());
            var map = new WorldMap(_mapName, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => map.RemovePlayer(player));
        }

        [Fact]
        public void WorldMapGetPlayerTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayer(Guid.NewGuid());
            var map = new WorldMap(_mapName, _serviceProvider);

            map.AddPlayer(player);
            var existingPlayer = map.GetPlayer(player.Id);

            Assert.NotNull(existingPlayer);
            Assert.Same(player, existingPlayer);
        }

        [Fact]
        public void WorldMapGetUnknownPlayerTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            Assert.Null(map.GetPlayer(Guid.NewGuid()));
        }

        [Fact]
        public void WorldMapStartStopUpdateTaskTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            map.StartUpdate();

            Assert.True(map.IsUpdating);

            map.StopUpdate();

            Assert.False(map.IsUpdating);
        }

        [Fact]
        public void WorldMapStartTwiceUpdateTaskTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            map.StartUpdate();

            Assert.Throws<InvalidOperationException>(() => map.StartUpdate());

            map.StopUpdate();
        }

        [Fact]
        public void WorldMapStopUpdateWithoutRunningTest()
        {
            var map = new WorldMap(_mapName, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => map.StopUpdate());
        }

        [Fact]
        public async Task WorldMapUpdatePlayersTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayerMock();
            var map = new WorldMap(_mapName, _serviceProvider);

            map.StartUpdate();
            map.AddPlayer(player.Object);

            await Task.Delay(500);

            player.Verify(x => x.KeepAlive(), Times.AtLeastOnce());
            player.Verify(x => x.LookAround(), Times.AtLeastOnce());

            map.StopUpdate();
        }

        [Fact]
        public async Task WorldMapDisposeTest()
        {
            var player = PlayerEntityGenerator.GeneratePlayer(Guid.NewGuid());
            var map = new WorldMap(_mapName, _serviceProvider);
            
            map.AddRegion(0, 0);
            map.AddPlayer(player);
            map.StartUpdate();

            await Task.Delay(500);

            map.Dispose();
            Assert.False(map.IsUpdating);
            Assert.Empty(map.Regions);
            Assert.Empty(map.Players);
        }
    }
}
