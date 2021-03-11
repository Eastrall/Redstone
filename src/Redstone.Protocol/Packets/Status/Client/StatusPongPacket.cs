using Redstone.Abstractions.Protocol;

namespace Redstone.Protocol.Packets.Status.Client
{
    /// <summary>
    /// Defines the Minecraft pong packet structure during <see cref="MinecraftUserStatus.Status"/> state.
    /// </summary>
    public class StatusPongPacket : MinecraftPacket
    {
        /// <summary>
        /// Creates a new <see cref="StatusPongPacket"/> instance.
        /// </summary>
        /// <param name="payload">Payload coming from client to be sent back again.</param>
        public StatusPongPacket(long payload)
            : base(ClientStatusPacketType.Pong)
        {
            WriteInt64(payload);
        }
    }
}
