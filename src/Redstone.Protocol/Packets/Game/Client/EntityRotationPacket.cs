namespace Redstone.Protocol.Packets.Game.Client;

public class EntityRotationPacket : MinecraftPacket
{
    public EntityRotationPacket(int entityId, float yawAngle, float pitchAngle, bool isOnGround)
        : base(ClientPlayPacketType.EntityRotation)
    {
        WriteVarInt32(entityId);
        WriteAngle(yawAngle);
        WriteAngle(pitchAngle);
        WriteBoolean(isOnGround);
    }
}
