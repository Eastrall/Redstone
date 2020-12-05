namespace Redstone.Common.Configuration
{
    /// <summary>
    /// Provides a data structure for configuring the Redstone server.
    /// </summary>
    public class ServerConfiguration
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
    }
}
