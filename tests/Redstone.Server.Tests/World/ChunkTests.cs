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
        public void CreateChunkWithNoServiceProviderTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Chunk(0, 0, null));
        }

        [Fact]
        public void ChunkFilledWithAirTest()
        {
            _registry.Load();

            var chunk = new Chunk(0, 0, _serviceProvider);

            Assert.NotNull(chunk);
            Assert.Equal(0, chunk.X);
            Assert.Equal(0, chunk.Z);

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
                x => x.CreateBlock(It.IsAny<BlockType>()), 
                Times.Exactly(ChunkSection.MaximumBlockAmount * chunk.Sections.Count()));
        }
    }
}
