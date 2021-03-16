using System;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides an abstraction to manage all minecraft world maps.
    /// </summary>
    public interface IWorldManager : IDisposable
    {
        /// <summary>
        /// Gets the world name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the overworld map.
        /// </summary>
        IWorldMap Overworld { get; }

        /// <summary>
        /// Gets the nether map.
        /// </summary>
        IWorldMap Nether { get; }

        /// <summary>
        /// Gets the end world map.
        /// </summary>
        IWorldMap End { get; }

        /// <summary>
        /// Loads the worlds for the given world name.
        /// </summary>
        /// <param name="worldName">World name to load.</param>
        void Load(string worldName);

        /// <summary>
        /// Saves the worlds.
        /// </summary>
        void Save();
    }
}
