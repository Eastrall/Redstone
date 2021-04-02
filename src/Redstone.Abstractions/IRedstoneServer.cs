using Redstone.Abstractions.Protocol;
using Redstone.Common.Server;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Redstone.Abstractions
{
    /// <summary>
    /// Provides an interface to manage the Redstone server instance.
    /// </summary>
    public interface IRedstoneServer
    {
        /// <summary>
        /// Gets the server events.
        /// </summary>
        IRedstoneServerEvents Events { get; }

        /// <summary>
        /// Gets the connected players list.
        /// </summary>
        IEnumerable<IMinecraftUser> ConnectedPlayers { get; }

        /// <summary>
        /// Gets the number of players connected.
        /// </summary>
        uint ConnectedPlayersCount { get; }

        /// <summary>
        /// Gets the server encryption key.
        /// </summary>
        RSAParameters ServerEncryptionKey { get; }

        /// <summary>
        /// Sends the given packet to a given collection of users.
        /// </summary>
        /// <param name="users">Users.</param>
        /// <param name="packet">Packet to send.</param>
        void SendTo(IEnumerable<IMinecraftUser> users, IMinecraftPacket packet);

        /// <summary>
        /// Gets the server status.
        /// </summary>
        /// <returns>Server status data structure.</returns>
        MinecraftServerStatus GetServerStatus();

        /// <summary>
        /// Gets an user identified by the given input username.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>The user if found; null otherwise.</returns>
        IMinecraftUser GetUser(string username);

        /// <summary>
        /// Checks if there is an user connected with the given input username.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>True if an user with the same username is already connected; false otherwise.</returns>
        bool HasUser(string username);

        /// <summary>
        /// Disconnects the minecraft user identified by the given UserId.
        /// </summary>
        /// <param name="userId">Minecraft user id.</param>
        void DisconnectUser(Guid userId);
    }
}
