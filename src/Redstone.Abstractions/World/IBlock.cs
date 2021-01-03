using Redstone.Common;
using Redstone.Common.Structures.Blocks;

namespace Redstone.Abstractions.World
{
    public interface IBlock
    {
        /// <summary>
        /// Gets a boolean value that indicates if the current block is an air block.
        /// </summary>
        bool IsAir { get; }
        
        /// <summary>
        /// Gets the block type.
        /// </summary>
        BlockType Type { get; }

        /// <summary>
        /// Gets the block position.
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Gets the current block state.
        /// </summary>
        BlockStateData State { get; }
    }
}
