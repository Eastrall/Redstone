using Redstone.Abstractions.Protocol;

namespace Redstone.Protocol.Packets.Status
{
    /// <summary>
    /// Defines the packets ids that can be sent to the client during the <see cref="MinecraftUserStatus.Status"/> state.
    /// </summary>
    public enum ClientStatusPacketType
    {
        Response = 0x00,
        Pong = 0x01
    }
}
