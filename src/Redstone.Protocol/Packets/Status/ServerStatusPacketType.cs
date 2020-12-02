namespace Redstone.Protocol.Packets.Status
{
    /// <summary>
    /// Defines the packets ids that can be received by the server during the <see cref="MinecraftUserStatus.Status"/> state.
    /// </summary>
    public enum ServerStatusPacketType
    {
        Request = 0x00,
        Ping = 0x01
    }
}
