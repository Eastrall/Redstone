using Redstone.Common.Server;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Redstone.Server
{
    /// <summary>
    /// Provides an interface to manage the Redstone server instance.
    /// </summary>
    public interface IRedstoneServer
    {
        IEnumerable<MinecraftUser> ConnectedPlayers { get; }

        uint ConnectedPlayersCount { get; }

        RSAParameters ServerEncryptionKey { get; }

        /// <summary>
        /// Gets the server status.
        /// </summary>
        /// <returns>Server status data structure.</returns>
        MinecraftServerStatus GetServerStatus();
    }
}
