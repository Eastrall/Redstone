using Redstone.Abstractions.Protocol;

namespace Redstone.Protocol.Packets.Handskake;

/// <summary>
/// Defines the packets ids that can be received by the server during the <see cref="MinecraftUserStatus.Handshaking"/> state.
/// </summary>
public enum ServerHandshakePacketType : int
{
    Handshaking = 0x00
}
