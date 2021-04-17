namespace Redstone.Common.Configuration
{
    /// <summary>
    /// Provides a data structure for configuring the Redstone server.
    /// </summary>
    public class ServerOptions
    {
        /// <summary>
        /// Gets or sets the server's listening IP address.
        /// </summary>
        /// <remarks>
        /// When null or empty, the server listen on all network interfaces.
        /// </remarks>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the server's listening port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of players connected at the same time on the server.
        /// </summary>
        public uint MaxPlayers { get; set; }

        /// <summary>
        /// Gets or sets the server's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the server's protocol version.
        /// </summary>
        public ushort ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the server's status description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a boolean that indicates if the server should use encryption.
        /// </summary>
        public bool UseEncryption { get; set; }

        /// <summary>
        /// Gets or sets the server mode.
        /// </summary>
        public ServerModeType Mode { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the server and client are running in debug mode.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the server and client should display reduced debug information.
        /// </summary>
        public bool ReducedDebugInfo { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the server is using a flat terrain.
        /// </summary>
        public bool FlatTerrain { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indiciates if the server allows debug using the same player name.
        /// </summary>
        /// <remarks>
        /// If a two players have the same name, an auto-incremented value will be append to the end of each player.
        /// </remarks>
        public bool AllowMultiplayerDebug { get; set; }
    }
}
