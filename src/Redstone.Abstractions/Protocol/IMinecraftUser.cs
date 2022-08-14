using Redstone.Abstractions.Entities;
using Redstone.Common.IO;
using System;

namespace Redstone.Abstractions.Protocol;

/// <summary>
/// Provides a mechanism to handle a miencraft user connection.
/// </summary>
public interface IMinecraftUser
{
    /// <summary>
    /// Gets the Minecraft user id.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the Miencraft user current status.
    /// </summary>
    MinecraftUserStatus Status { get; }

    /// <summary>
    /// Gets the Minecraft user name.
    /// </summary>
    string Username { get; }

    /// <summary>
    /// Gets the Minecraft user's player information.
    /// </summary>
    IPlayer Player { get; }

    /// <summary>
    /// Updates the Miencraft user's status.
    /// </summary>
    /// <param name="newStatus">User status.</param>
    void UpdateStatus(MinecraftUserStatus newStatus);

    /// <summary>
    /// Sends the given packet to the current user.
    /// </summary>
    /// <param name="packet">Packet to be sent.</param>
    void Send(IMinecraftPacket packet);

    /// <summary>
    /// Disconnects the current user.
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Disconnects the current user with a given reason.
    /// </summary>
    /// <param name="reason">Disconnect reason.</param>
    void Disconnect(string reason);

    /// <summary>
    /// Loads the current player's information.
    /// </summary>
    /// <param name="playerId">Player id to be loaded.</param>
    /// <param name="playerName">Player name to be loaded.</param>
    void LoadPlayer(Guid playerId, string playerName);
}
