using Redstone.Common;
using System.Collections.Generic;

namespace Redstone.Abstractions.World
{
    public interface IChunk
    {
        int X { get; }

        int Z { get; }

        IEnumerable<long> Heightmap { get; }

        IEnumerable<long> WorldSurfaceHeightmap { get; }

        IEnumerable<IChunkSection> Sections { get; }

        void GenerateHeightMap();

        IBlock GetBlock(int x, int y, int z);

        IChunkSection GetSection(int sectionIndex);

        void SetBlock(BlockType blockType, int x, int y, int z);

        void SetBlock(IBlock block, int x, int y, int z);
    }
}
