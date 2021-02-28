namespace Redstone.Protocol.Packets.Game
{
    public enum ClientPlayPacketType : byte
    {
        PluginMessage = 0x17,
        KeepAlive = 0x1F,
        ChunkData = 0x20,
        JoinGame = 0x24,
        PlayerInfo = 0x32,
        PlayerPositionAndLook = 0x34,
        UpdateViewPosition = 0x40,
        SpawnPosition = 0x42
    }
}
