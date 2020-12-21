using System.Collections.Generic;

namespace Redstone.Abstractions.World
{
    public interface IChunk
    {
        int X { get; }

        int Z { get; }

        IEnumerable<IChunkSection> Sections { get; }

        IChunkSection GetSection(int sectionIndex);
    }
}
