using Redstone.Protocol.Abstractions;

namespace Redstone.Protocol.Packets.Status.Server
{
    /// <summary>
    /// Defines the Minecraft status ping packet structure.
    /// </summary>
    public class StatusPingPacket
    {
        /// <summary>
        /// Gets the ping payload.
        /// </summary>
        public long Payload { get; private set; }

        /// <summary>
        /// Creates a new <see cref="StatusPingPacket"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public StatusPingPacket(IMinecraftPacket packet)
        {
            Payload = packet.ReadInt64();
        }
    }
}
