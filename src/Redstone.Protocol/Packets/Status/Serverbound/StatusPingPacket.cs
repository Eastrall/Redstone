using Redstone.Protocol.Abstractions;

namespace Redstone.Protocol.Packets.Status.Serverbound
{
    public class StatusPingPacket
    {
        /// <summary>
        /// Gets the ping payload.
        /// </summary>
        public long Payload { get; private set; }

        public StatusPingPacket(IMinecraftPacket packet)
        {
            Payload = packet.ReadInt64();
        }
    }
}
