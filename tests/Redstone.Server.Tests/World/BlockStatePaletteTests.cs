using Redstone.Server.World;
using Redstone.Server.World.Palettes;
using System;
using Xunit;

namespace Redstone.Server.Tests.World
{
    public class BlockStatePaletteTests
    {
        private readonly BlockStatePalette _palette;

        public BlockStatePaletteTests()
        {
            _palette = new BlockStatePalette(ChunkSection.DefaultBitsPerBlock, ChunkSection.MaximumBitsPerBlock);
        }

        [Fact]
        public void SetBlockStateTest()
        {
            int paletteIndex = _palette.SetState(1);

            Assert.Equal(0, paletteIndex);
            Assert.False(_palette.IsFull);
            Assert.Equal(1, _palette.Count);
        }

        [Fact]
        public void SetMultipleBlockStatesTest()
        {
            int airPaletteIndex = _palette.SetState(0);
            int otherBlockPaletteIndex = _palette.SetState(1);

            Assert.Equal(0, airPaletteIndex);
            Assert.Equal(1, otherBlockPaletteIndex);
            Assert.False(_palette.IsFull);
            Assert.Equal(2, _palette.Count);
        }

        [Fact]
        public void SetBlockStateAndResizePaletteTest()
        {
            bool resizedCalled = false;
            int resizeBits = 0;

            _palette.Resized += (sender, bits) =>
            {
                resizedCalled = true;
                resizeBits = bits;
            };

            for (int i = 0; i < 16; i++)
            {
                _palette.SetState(i);
            }

            Assert.True(_palette.IsFull);
            Assert.Equal(16, _palette.Count);

            int paletteIndex = _palette.SetState(42);

            Assert.Equal(16, paletteIndex);
            Assert.True(resizedCalled);
            Assert.Equal(5, resizeBits);
            Assert.True(_palette.IsFull);
            Assert.Equal(17, _palette.Count);
        }

        [Fact]
        public void GetBlockStateTest()
        {
            const int BlockStateId = 1;

            int paletteIndex = _palette.SetState(BlockStateId);
            int paletteBlockState = _palette.GetState(paletteIndex);

            Assert.Equal(0, paletteIndex);
            Assert.Equal(BlockStateId, paletteBlockState);
        }

        [Fact]
        public void GetUnknownBlockStateTest()
        {
            int paletteBlockState = _palette.GetState(5);

            Assert.Equal(-1, paletteBlockState);
        }

        [Fact]
        public void GetBlockStateOutOfRangeTest()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _palette.GetState(int.MaxValue));
        }
    }
}
