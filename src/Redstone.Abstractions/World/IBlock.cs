using Redstone.Common;
using Redstone.Common.Structures.Blocks;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Represents a block in the Minecraft world.
    /// </summary>
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
        /// Gets the X position within the chunk.
        /// </summary>
        int X { get; }

        /// <summary>
        /// Gets the Y position within the chunk.
        /// </summary>
        int Y { get; }

        /// <summary>
        /// Gets the Z position within the chunk.
        /// </summary>
        int Z { get; }

        /// <summary>
        /// Gets the parent chunk.
        /// </summary>
        IChunk Chunk { get; }

        /// <summary>
        /// Gets the current block state.
        /// </summary>
        BlockStateData State { get; }

        /// <summary>
        /// Sets the block type.
        /// </summary>
        /// <param name="type">Block type.</param>
        void SetType(BlockType type);

        /// <summary>
        /// Sets the block state.
        /// </summary>
        /// <param name="state">Block state.</param>
        void SetState(BlockStateData state);

        /// <summary>
        /// Gets the relative block of the current block.
        /// </summary>
        /// <param name="blockFace">Block face.</param>
        /// <returns>The relative block.</returns>
        IBlock GetRelative(BlockFaceType blockFace);
    }
}
