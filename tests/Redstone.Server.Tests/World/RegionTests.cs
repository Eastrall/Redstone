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
        private readonly Mock<IWorldMap> _worldMapMock;

        public RegionTests()
        {
            _worldMapMock = new();
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
                .AddSingleton(s => _blockFactoryMock.Object)
                .AddSingleton(s => _registry)
                .BuildServiceProvider();
        }

        [Fact]
        public void CreateRegionTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);

            Assert.NotNull(region);
            Assert.NotEmpty(region.Chunks);
            Assert.Equal(Region.ChunkAmount * Region.ChunkAmount, region.Chunks.Count());
            Assert.Equal(0, region.X);
            Assert.Equal(0, region.Z);
        }

        [Fact]
        public void CreateRegionWithoutServiceProviderTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Region(_worldMapMock.Object, 0, 0, null));
        }

        [Fact]
        public void RegionCreateChunkTest()
        {
            IRegion region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);
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
            IRegion region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);
            IChunk chunk = region.AddChunk(0, 0);

            Assert.Throws<InvalidOperationException>(() => region.AddChunk(0, 0));
        }

        [Fact]
        public void RegionCheckIfContainsChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);
            region.AddChunk(0, 0);

            Assert.True(region.ContainsChunk(0, 0));
        }
        
        [Fact]
        public void RegionCheckIfDoesntContainsChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);

            Assert.False(region.ContainsChunk(0, 0));
        }

        [Fact]
        public void RegionGetExistingChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);
            IChunk chunk = region.AddChunk(0, 0);

            Assert.NotNull(chunk);
            Assert.Same(chunk, region.GetChunk(0, 0));
        }

        [Fact]
        public void RegionGetUnknownChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);

            Assert.Null(region.GetChunk(0, 0));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(17, 6, 20)]
        [InlineData(4, 247, 16)]
        public void RegionGetBlockTest(int x, int y, int z)
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);
            region.AddChunk(0, 0);
            region.AddChunk(1, 0);
            region.AddChunk(0, 1);
            region.AddChunk(1, 1);

            IBlock block = region.GetBlock(x, y, z);

            Assert.NotNull(block);
            Assert.IsType<Block>(block);
            Assert.Equal(BlockType.Air, block.Type);
            Assert.True(block.IsAir);
        }

        [Fact]
        public void RegionGetBlockAtUnknownChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => region.GetBlock(0, 0, 0));
        }

        [Fact]
        public void RegionSetBlockTypeAtUnknownChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => region.SetBlock(BlockType.GrassBlock, 0, 0, 0));
        }

        [Theory]
        [InlineData(0, 0, 0, BlockType.Stone)]
        [InlineData(17, 6, 20, BlockType.GrassBlock)]
        [InlineData(4, 247, 16, BlockType.Basalt)]
        public void RegionSetBlockAtChunkTest(int x, int y, int z, BlockType blockType)
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);
            region.AddChunk(0, 0);
            region.AddChunk(1, 0);
            region.AddChunk(0, 1);
            region.AddChunk(1, 1);

            IBlock placedBlock = region.SetBlock(blockType, x, y, z);
            IBlock block = region.GetBlock(x, y, z);

            Assert.NotNull(placedBlock);
            Assert.NotNull(block);
            Assert.Equal(placedBlock, block);
            Assert.IsType<Block>(placedBlock);
            Assert.IsType<Block>(block);
            Assert.Equal(blockType, block.Type);
            Assert.False(block.IsAir);
        }

        [Fact]
        public void RegionSetBlockAtUnknownChunkTest()
        {
            var region = new Region(_worldMapMock.Object, 0, 0, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => region.SetBlock(BlockType.GrassBlock, 0, 0, 0));
        }
    }
}
