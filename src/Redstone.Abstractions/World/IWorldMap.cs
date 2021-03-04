using Redstone.Abstractions.Entities;
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
        /// Gets the players present in the current world map.
        /// </summary>
        IEnumerable<IPlayer> Players { get; }

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

        /// <summary>
        /// Adds a player to the world map.
        /// </summary>
        /// <param name="player">Player to add.</param>
        void AddPlayer(IPlayer player);

        /// <summary>
        /// Removes a player from the current world map.
        /// </summary>
        /// <param name="player">Player to remove.</param>
        /// <returns>Removed player.</returns>
        IPlayer RemovePlayer(IPlayer player);

        /// <summary>
        /// Gets a player from the current world map.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        /// <returns>Player if found; null otherwise.</returns>
        IPlayer GetPlayer(Guid playerId);

        /// <summary>
        /// Gets a collection with the visible entities for the given entity.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        /// <returns>Collection of visible entities <see cref="IEntity"/> for the current entity.</returns>
        IEnumerable<IEntity> GetVisibleEntities(IEntity entity);

        /// <summary>
        /// Starts the world map update process.
        /// </summary>
        void StartUpdate();

        /// <summary>
        /// Stops the world map update process.
        /// </summary>
        void StopUpdate();
    }
}
