using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Common.IO;
using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.World;

/// <summary>
/// Provides an abstraction to manage a minecraft world map.
/// </summary>
public interface IWorldMap : IDisposable
{
    /// <summary>
    /// Gets a boolean value that indicates if the current map is being updated.
    /// </summary>
    bool IsUpdating { get; }

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
    /// Gets a block within the world map.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <returns>The block at given coordinates.</returns>
    IBlock GetBlock(int x, int y, int z);

    /// <summary>
    /// Gets a block whithin the world map.
    /// </summary>
    /// <param name="position">Block position.</param>
    /// <returns>The block at the given position.</returns>
    IBlock GetBlock(Position position);

    /// <summary>
    /// Sets a block within the world map.
    /// </summary>
    /// <param name="blockType">Block type to set.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <returns>The block placed at the given position.</returns>
    IBlock SetBlock(BlockType blockType, int x, int y, int z);

    /// <summary>
    /// Sets a block within the world map.
    /// </summary>
    /// <param name="blockType">Block type to set.</param>
    /// <param name="position">Block position.</param>
    /// <returns>The block placed at the given position.</returns>
    IBlock SetBlock(BlockType blockType, Position position);

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

    /// <summary>
    /// Broadcasts a packet to every connected players on the current map.
    /// </summary>
    /// <param name="packet">Packet to broadcast.</param>
    void Broadcast(IMinecraftPacket packet);
}
