using System.Collections.Generic;

namespace Redstone.Abstractions.World
{
    public interface IRegion
    {
        int X { get; }

        int Z { get; }

        IEnumerable<IChunk> Chunks { get; }

        IChunk GetChunk(int x, int z);
    }
}
