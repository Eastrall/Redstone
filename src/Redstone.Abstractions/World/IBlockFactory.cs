namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides a mechanism to create blocks.
    /// </summary>
    public interface IBlockFactory
    {
        /// <summary>
        /// Creates a <typeparamref name="TBlock"/> instance at default position (0,0,0).
        /// </summary>
        /// <typeparam name="TBlock">Block type.</typeparam>
        /// <returns>New block.</returns>
        TBlock CreateBlock<TBlock>() where TBlock : class, IBlock;

        /// <summary>
        /// Creates a new <typeparamref name="TBlock"/>> instance at the given position.
        /// </summary>
        /// <typeparam name="TBlock">Block type.</typeparam>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>New block.</returns>
        TBlock CreateBlock<TBlock>(int x, int y, int z) where TBlock : class, IBlock;
    }
}
