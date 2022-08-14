using Redstone.Abstractions.Protocol;

namespace Redstone.Protocol.Packets.Handskake.Server;

/// <summary>
/// Defines the Minecraft handshake packet structure.
/// </summary>
public class HandshakePacket
{
    /// <summary>
    /// Gets the Minecraft protocol version.
    /// </summary>
    public int ProtocolVersion { get; private set; }

    /// <summary>
    /// Gets the server address.
    /// </summary>
    public string ServerAddress { get; private set; }

    /// <summary>
    /// Gets the server port.
    /// </summary>
    public ushort ServerPort { get; private set; }

    /// <summary>
    /// Gets the next user state.
    /// </summary>
    public MinecraftUserStatus NextState { get; private set; }

    /// <summary>
    /// Creates a new <see cref="HandshakePacket"/> instance.
    /// </summary>
    /// <param name="packet">Incoming packet from client.</param>
    public HandshakePacket(IMinecraftPacket packet)
    {
        ProtocolVersion = packet.ReadVarInt32();
        ServerAddress = packet.ReadString();
        ServerPort = packet.ReadUInt16();
        NextState = (MinecraftUserStatus)packet.ReadVarInt32();
    }
}
