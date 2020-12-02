namespace Redstone.Common
{
    public class ServerConfiguration
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public uint MaxPlayers { get; set; }

        public string Description { get; set; }

        public bool UseEncryption { get; set; }

        public ServerModeType Mode { get; set; }
    }
}
