using System;

namespace Redstone.Common.Exceptions;

public class BlockNotFoundException : Exception
{
    public Position BlockPosition { get; }

    public BlockNotFoundException(int x, int y, int z)
        : base($"Failed to find block at position: {x},{y},{z}")
    {
        BlockPosition = new Position(x, y, z);
    }
}
