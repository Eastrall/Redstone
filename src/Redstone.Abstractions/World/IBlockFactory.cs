using Redstone.Common;

namespace Redstone.Abstractions.World;

/// <summary>
/// Provides a mechanism to create blocks.
/// </summary>
public interface IBlockFactory
{
    /// <summary>
    /// Creates a new block of the given type.
    /// </summary>
    /// <param name="blockType">Block type.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <param name="chunk">Parent chunk of the block to create.</param>
    /// <returns>New block.</returns>
    IBlock CreateBlock(BlockType blockType, int x, int y, int z, IChunk chunk);
}
