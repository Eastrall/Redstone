using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides an abstraction to manage a minecraft world map.
    /// </summary>
    public interface IWorldMap
    {
        /// <summary>
        /// Gets the world map name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the world map regions.
        /// </summary>
        IEnumerable<IRegion> Regions { get; }

        /// <summary>
        /// Adds a new region at the given x,z position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>The newly created region at the given position.</returns>
        /// <exception cref="InvalidOperationException">The region already exists.</exception>
        IRegion AddRegion(int x, int z);

        /// <summary>
        /// Gets a region at the given x,z position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>The region at the given position; null otherwise.</returns>
        IRegion GetRegion(int x, int z);

        /// <summary>
        /// Checks if the world map has a region at the given x,z position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>True if the region exists; false otherwise.</returns>
        bool ContainsRegion(int x, int z);
    }
}
