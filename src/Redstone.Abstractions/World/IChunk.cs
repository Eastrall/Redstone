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

        IChunkSection GetSection(int sectionIndex);

        void GenerateHeightMap();

        void SetBlock(IBlock block, int x, int y, int z);

        IBlock GetBlock(int x, int y, int z);
    }
}
