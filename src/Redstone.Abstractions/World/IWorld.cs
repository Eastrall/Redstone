using Redstone.Abstractions.Protocol;
using Redstone.Common.IO;
using System;

namespace Redstone.Abstractions.World;

/// <summary>
/// Provides a mechanism that represents a Minecraft world with several world maps.
/// </summary>
public interface IWorld : IDisposable
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

    /// <summary>
    /// Sends the given packet to every players on the world.
    /// </summary>
    /// <param name="packet">Packet to send.</param>
    void SendToAll(IMinecraftPacket packet);
}
