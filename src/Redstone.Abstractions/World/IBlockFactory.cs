using Redstone.Common;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides a mechanism to create blocks.
    /// </summary>
    public interface IBlockFactory
    {
        /// <summary>
        /// Creates a new block of the given type.
        /// </summary>
        /// <param name="blockType">Block type.</param>
        /// <returns>New block.</returns>
        IBlock CreateBlock(BlockType blockType);
    }
}
