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
        /// Gets a boolean value that indicates if the current block is a fluid block.
        /// </summary>
        bool IsFluid { get; }

        /// <summary>
        /// Gets a boolean value that indicate if the current block is a solid block.
        /// </summary>
        bool IsSolid { get; }
     
        /// <summary>
        /// Gets the block id.
        /// </summary>
        int Id { get; }

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
