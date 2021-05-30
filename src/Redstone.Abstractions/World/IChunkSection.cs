using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Common.IO;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides a mechanism to manipulate a chunk section.
    /// </summary>
    public interface IChunkSection : INetworkSerializable
    {
        /// <summary>
        /// Gets a value that indicates if the current chunk section is dirty.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Gets the chunk section index.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets a block at a given x,y,z position in the current chunk section.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns></returns>
        IBlock GetBlock(int x, int y, int z);

        /// <summary>
        /// Sets a block type at a given x,y,z position in the current chunk section.
        /// </summary>
        /// <param name="blockType">Block type.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        void SetBlock(BlockType blockType, int x, int y, int z);

        /// <summary>
        /// Sets a block at a given x,y,z position in the current chunk section.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        void SetBlock(IBlock block, int x, int y, int z);

        /// <summary>
        /// Gets the total amount of non-air blocks in the current chunk section.
        /// </summary>
        /// <returns></returns>
        short GetBlockAmount();
    }
}
