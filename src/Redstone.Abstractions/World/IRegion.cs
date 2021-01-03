using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides an abstraction to manage a minecraft region.
    /// </summary>
    public interface IRegion
    {
        /// <summary>
        /// Gets the region X position in the world.
        /// </summary>
        int X { get; }

        /// <summary>
        /// Gets the region Z position in the world.
        /// </summary>
        int Z { get; }

        /// <summary>
        /// Gets the chunks of the current region.
        /// </summary>
        IEnumerable<IChunk> Chunks { get; }

        /// <summary>
        /// Gets a chunk at a given x,z position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>Chunk at the given position.</returns>
        /// <exception cref="IndexOutOfRangeException">The chunk index is out of range.</exception>
        IChunk GetChunk(int x, int z);

        /// <summary>
        /// Adds a chunk at a given x,z position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>New created chunk instance at the given position.</returns>
        /// <exception cref="InvalidOperationException">The chunk already exists.</exception>
        IChunk AddChunk(int x, int z);

        /// <summary>
        /// Checks if their is a chunk at the given x,z position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>True if the chunk exists; false otherwise.</returns>
        bool ContainsChunk(int x, int z);
    }
}
