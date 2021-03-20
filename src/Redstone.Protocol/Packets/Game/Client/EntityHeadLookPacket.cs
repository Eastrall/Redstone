namespace Redstone.Protocol.Packets.Game.Client
{
    public class EntityHeadLookPacket : MinecraftPacket
    {
        public EntityHeadLookPacket(int entityId, float yawAngle)
            : base(ClientPlayPacketType.EntityHeadLook)
        {
            WriteVarInt32(entityId);
            WriteAngle(yawAngle);
        }
    }
}
