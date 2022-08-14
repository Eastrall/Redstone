using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Structures.Blocks;
using Redstone.Server.Registry;
using Redstone.Server.Tests.Mocks;
using Redstone.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Redstone.Server.Tests.World;

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
        _blockFactoryMock.Setup(x => x.CreateBlock(It.IsAny<BlockType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IChunk>()))
                         .Returns<BlockType, int, int, int, IChunk>((type, x, y, z, chunk) =>
                         {
                             return new Block(x, y, z, chunk, new BlockData(type.ToString(), (int)type, null, new[]
                             {
                                 new BlockStateData((int)type, true, new Dictionary<string, string>())
                             }), _registry);
                         });
        _registry = new DataRegistry(new Mock<ILogger<DataRegistry>>().Object);
        _registry.Load();
        _serviceProvider = new ServiceCollection()
            .AddSingleton<ILogger<WorldMap>>(new Mock<ILogger<WorldMap>>().Object)
            .AddSingleton(s => _blockFactoryMock.Object)
            .AddSingleton(s => _registry)
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
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
        var map = new WorldMap(_mapName, _serviceProvider);

        map.AddPlayer(player);

        Assert.NotEmpty(map.Players);
        Assert.Single(map.Players);
        Assert.Same(map, player.Map);
    }

    [Fact]
    public void WorldMapAddExistingPlayerTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
        var map = new WorldMap(_mapName, _serviceProvider);

        map.AddPlayer(player);

        Assert.NotEmpty(map.Players);
        Assert.Single(map.Players);
        Assert.Throws<InvalidOperationException>(() => map.AddPlayer(player));
    }

    [Fact]
    public void WorldMapRemovePlayerTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
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
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
        var map = new WorldMap(_mapName, _serviceProvider);

        Assert.Throws<InvalidOperationException>(() => map.RemovePlayer(player));
    }

    [Fact]
    public void WorldMapGetPlayerTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
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
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
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

    [Theory]
    [InlineData(0, 183, 0)]
    [InlineData(17, 47, 20)]
    public void WorldMapGetBlockTest(int x, int y, int z)
    {
        var map = new WorldMap(_mapName, _serviceProvider);
        IRegion region0 = map.AddRegion(0, 0);
        region0.AddChunk(0, 0);
        region0.AddChunk(1, 0);
        region0.AddChunk(0, 1);
        region0.AddChunk(1, 1);

        IBlock block = map.GetBlock(x, y, z);

        Assert.NotNull(block);
        Assert.IsType<Block>(block);
        Assert.Equal(BlockType.Air, block.Type);
        Assert.True(block.IsAir);
    }

    public static IEnumerable<object[]> GetPositions => new[]
    {
        new[] { new Position(0, 183, 0) },
        new[] { new Position(17, 47, 20) }
    };

    [Theory]
    [MemberData(nameof(GetPositions))]
    public void WorldMapGetBlockAtPositionTest(Position position)
    {
        var map = new WorldMap(_mapName, _serviceProvider);
        IRegion region0 = map.AddRegion(0, 0);
        region0.AddChunk(0, 0);
        region0.AddChunk(1, 0);
        region0.AddChunk(0, 1);
        region0.AddChunk(1, 1);

        IBlock block = map.GetBlock(position);

        Assert.NotNull(block);
        Assert.IsType<Block>(block);
        Assert.Equal(BlockType.Air, block.Type);
        Assert.True(block.IsAir);
    }

    [Fact]
    public void WorldMapGetBlockAtUnknownRegion()
    {
        var map = new WorldMap(_mapName, _serviceProvider);

        Assert.Throws<InvalidOperationException>(() => map.GetBlock(0, 0, 0));
    }

    [Fact]
    public void WorldMapSetBlockTypeAtUnknownRegionTest()
    {
        var map = new WorldMap(_mapName, _serviceProvider);

        Assert.Throws<InvalidOperationException>(() => map.SetBlock(BlockType.GrassBlock, 0, 0, 0));
    }

    [Theory]
    [InlineData(0, 183, 0, BlockType.GrassBlock)]
    [InlineData(17, 47, 20, BlockType.Dirt)]
    public void WorldMapSetBlockTest(int x, int y, int z, BlockType blockType)
    {
        var map = new WorldMap(_mapName, _serviceProvider);
        IRegion region0 = map.AddRegion(0, 0);
        region0.AddChunk(0, 0);
        region0.AddChunk(1, 0);
        region0.AddChunk(0, 1);
        region0.AddChunk(1, 1);

        map.SetBlock(blockType, x, y, z);
        IBlock block = map.GetBlock(x, y, z);

        Assert.NotNull(block);
        Assert.IsType<Block>(block);
        Assert.Equal(blockType, block.Type);
        Assert.False(block.IsAir);
    }

    [Theory]
    [InlineData(0, 183, 0, BlockType.GrassBlock)]
    [InlineData(17, 47, 20, BlockType.Dirt)]
    public void WorldMapSetBlockAtPositionTest(int x, int y, int z, BlockType blockType)
    {
        var position = new Position(x, y, z);
        var map = new WorldMap(_mapName, _serviceProvider);
        IRegion region0 = map.AddRegion(0, 0);
        region0.AddChunk(0, 0);
        region0.AddChunk(1, 0);
        region0.AddChunk(0, 1);
        region0.AddChunk(1, 1);

        map.SetBlock(blockType, position);
        IBlock block = map.GetBlock(position);

        Assert.NotNull(block);
        Assert.IsType<Block>(block);
        Assert.Equal(blockType, block.Type);
        Assert.False(block.IsAir);
    }

    [Fact]
    public void WorldMapSetBlockAtUnknownRegionTest()
    {
        var map = new WorldMap(_mapName, _serviceProvider);

        Assert.Throws<InvalidOperationException>(() => map.SetBlock(BlockType.GrassBlock, 0, 0, 0));
    }
}
