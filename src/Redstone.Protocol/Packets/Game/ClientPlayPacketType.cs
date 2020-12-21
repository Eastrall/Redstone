namespace Redstone.Protocol.Packets.Game
{
    public enum ClientPlayPacketType : byte
    {
        ChunkData = 0x20,
        JoinGame = 0x24,
        PlayerInfo = 0x32,
        UpdateViewPosition = 0x40
    }
}
