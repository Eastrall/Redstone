using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Exceptions;
using Redstone.Common.Structures.Blocks;
using Redstone.Server.Registry;
using Redstone.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Redstone.Server.Tests.World
{
    /// <summary>
    /// Tests the chunk mechanism.
    /// </summary>
    public class ChunkTests
    {
        private readonly IRegistry _registry;
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<IBlockFactory> _blockFactoryMock;

        public ChunkTests()
        {
            _blockFactoryMock = new Mock<IBlockFactory>();
            _blockFactoryMock.Setup(x => x.CreateBlock(It.IsAny<BlockType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IChunk>()))
                             .Returns<BlockType, int, int, int, IChunk>((type, x, y, z, chunk) =>
                             {
                                 return new Block(x, y, z, chunk, new BlockData(type.ToString(), null, new[]
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
        public void CreateChunkWithNoServiceProviderTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Chunk(0, 0, null));
        }

        [Fact]
        public void ChunkFilledWithAirTest()
        {
            var chunk = new Chunk(0, 0, _serviceProvider);

            Assert.NotNull(chunk);
            Assert.Equal(0, chunk.X);
            Assert.Equal(0, chunk.Z);
            Assert.NotEmpty(chunk.Sections);

            foreach (IChunkSection section in chunk.Sections)
            {
                for (int x = 0; x < ChunkSection.Size; x++)
                {
                    for (int y = 0; y < ChunkSection.Size; y++)
                    {
                        for (int z = 0; z < ChunkSection.Size; z++)
                        {
                            IBlock block = section.GetBlock(x, y, z);

                            Assert.NotNull(block);
                            Assert.Equal(BlockType.Air, block.Type);
                            Assert.True(block.IsAir);
                        }
                    }
                }
            }

            _blockFactoryMock.Verify(
                x => x.CreateBlock(It.IsAny<BlockType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IChunk>()), 
                Times.Exactly(ChunkSection.MaximumBlockAmount * chunk.Sections.Count()));
        }

        [Fact]
        public void ChunkGetBlockTest()
        {
            var chunk = new Chunk(0, 0, _serviceProvider);
            IBlock block = chunk.GetBlock(0, 0, 0);

            Assert.NotNull(block);
            Assert.IsType<Block>(block);
            Assert.Equal(BlockType.Air, block.Type);
            Assert.True(block.IsAir);
            Assert.False(block.IsFluid);
            Assert.False(block.IsSolid);
            Assert.Equal(0, block.X);
            Assert.Equal(0, block.Y);
            Assert.Equal(0, block.Z);
            Assert.Equal(chunk, block.Chunk);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(300)]
        [InlineData(-300)]
        public void ChunkGetBlockAtInvalidSectionTest(int height)
        {
            var chunk = new Chunk(0, 0, _serviceProvider);
            
            Assert.Throws<InvalidOperationException>(() => chunk.GetBlock(0, height, 0));
        }

        [Fact]
        public void ChunkGetSectionTest()
        {
            var chunk = new Chunk(0, 0, _serviceProvider);
            IChunkSection section = chunk.GetSection(0);

            Assert.NotNull(section);
            Assert.IsType<ChunkSection>(section);
            Assert.Equal(0, section.Index);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(300)]
        [InlineData(-300)]
        public void ChunkGetInvalidChunkSectionTest(int sectionIndex)
        {
            var chunk = new Chunk(0, 0, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => chunk.GetSection(sectionIndex));
        }

        [Fact]
        public void ChunkSetBlockTest()
        {
            var chunk = new Chunk(0, 0, _serviceProvider);
            chunk.SetBlock(BlockType.Dirt, 0, 1, 0);

            IBlock dirtBlock = chunk.GetBlock(0, 1, 0);

            Assert.NotNull(dirtBlock);
            Assert.IsType<Block>(dirtBlock);
            Assert.False(dirtBlock.IsAir);
            Assert.False(dirtBlock.IsFluid);
            Assert.Equal(BlockType.Dirt, dirtBlock.Type);
            Assert.Equal(1, chunk.Heightmap.ElementAt(0));
        }

        [Theory]
        [InlineData(34, 48, 0)]
        [InlineData(0, 23, 49)]
        [InlineData(19, 20, 24)]
        [InlineData(-12, 38, 0)]
        [InlineData(0, 38, -12)]
        [InlineData(2, -20, 11)]
        [InlineData(2, 399, 11)]
        public void ChunkSetBlockAtInvalidPositionTest(int x, int y, int z)
        {
            var chunk = new Chunk(0, 0, _serviceProvider);

            Assert.Throws<InvalidOperationException>(() => chunk.SetBlock(BlockType.Dirt, x, y, z));
        }

        [Fact]
        public void ChunkGenerateEmptyHeightMapTest()
        {
            var chunk = new Chunk(0, 0, _serviceProvider);

            chunk.GenerateHeightMap();

            for (int i = 0; i < chunk.Heightmap.Count(); i++)
            {
                Assert.Equal(0, chunk.Heightmap.ElementAt(i));
            }
        }
    }
}
