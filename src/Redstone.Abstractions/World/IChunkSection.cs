using Redstone.Protocol.Abstractions;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides a mechanism to manipulate a chunk section.
    /// </summary>
    public interface IChunkSection : INetworkSerializable
    {
        /// <summary>
        /// Gets the chunk section index.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets a block at a given x,y,z position in the current chunk section.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        IBlock GetBlock(int x, int y, int z);

        /// <summary>
        /// Sets a block at a given x,y,z position in the current chunk section.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void SetBlock(IBlock block, int x, int y, int z);

        /// <summary>
        /// Gets the total amount of non-air blocks in the current chunk section.
        /// </summary>
        /// <returns></returns>
        int GetBlockAmount();
    }
}
