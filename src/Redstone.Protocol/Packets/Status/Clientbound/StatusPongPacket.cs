namespace Redstone.Protocol.Packets.Status.Clientbound
{
    public class StatusPongPacket : MinecraftPacket
    {
        public StatusPongPacket(long payload)
            : base(StatusClientboundPacketType.Pong)
        {
            WriteInt64(payload);
        }
    }
}
