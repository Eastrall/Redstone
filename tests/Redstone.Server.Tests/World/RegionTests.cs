using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Structures.Blocks;
using Redstone.Server.Registry;
using Redstone.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Redstone.Server.Tests.World
{
    public class RegionTests
    {
        private readonly IRegistry _registry;
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<IBlockFactory> _blockFactoryMock;

        public RegionTests()
        {
            _blockFactoryMock = new Mock<IBlockFactory>();
            _blockFactoryMock.Setup(x => x.CreateBlock(It.IsAny<BlockType>()))
                             .Returns<BlockType>(x =>
                             {
                                 return new Block(new BlockData(x.ToString(), null, new[]
                                 {
                                    new BlockStateData((int)x, true, new Dictionary<string, string>())
                                 }));
                             });
            _registry = new DataRegistry(new Mock<ILogger<DataRegistry>>().Object);
            _serviceProvider = new ServiceCollection()
                .AddSingleton(s => _blockFactoryMock.Object)
                .BuildServiceProvider();
        }

        [Fact]
        public void CreateRegionTest()
        {
            var region = new Region(0, 0, _serviceProvider);

            Assert.NotNull(region);
            Assert.NotEmpty(region.Chunks);
            Assert.Equal(Region.ChunkAmount * Region.ChunkAmount, region.Chunks.Count());
            Assert.Equal(0, region.X);
            Assert.Equal(0, region.Z);
        }

        [Fact]
        public void CreateRegionWithoutServiceProviderTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Region(0, 0, null));
        }

        [Fact]
        public void RegionCreateChunkTest()
        {
            IRegion region = new Region(0, 0, _serviceProvider);
            IChunk chunk = region.AddChunk(0, 0);

            Assert.NotNull(chunk);
            Assert.IsType<Chunk>(chunk);
            Assert.Equal(0, chunk.X);
            Assert.Equal(0, chunk.Z);
            Assert.NotEmpty(region.Chunks.Where(x => x is not null));
        }

        [Fact]
        public void RegionCreateExistingChunkTest()
        {
            IRegion region = new Region(0, 0, _serviceProvider);
            IChunk chunk = region.AddChunk(0, 0);

            Assert.Throws<InvalidOperationException>(() => region.AddChunk(0, 0));
        }

        [Fact]
        public void RegionCheckIfContainsChunkTest()
        {
            var region = new Region(0, 0, _serviceProvider);
            region.AddChunk(0, 0);

            Assert.True(region.ContainsChunk(0, 0));
        }
        
        [Fact]
        public void RegionCheckIfDoesntContainsChunkTest()
        {
            var region = new Region(0, 0, _serviceProvider);

            Assert.False(region.ContainsChunk(0, 0));
        }

        [Fact]
        public void RegionGetExistingChunkTest()
        {
            var region = new Region(0, 0, _serviceProvider);
            IChunk chunk = region.AddChunk(0, 0);

            Assert.NotNull(chunk);
            Assert.Same(chunk, region.GetChunk(0, 0));
        }

        [Fact]
        public void RegionGetUnknownChunkTest()
        {
            var region = new Region(0, 0, _serviceProvider);

            Assert.Null(region.GetChunk(0, 0));
        }
    }
}
