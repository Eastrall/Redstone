using Redstone.Abstractions.Entities;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class EntityPositionAndRotationPacket : MinecraftPacket
    {
        public EntityPositionAndRotationPacket(IEntity entity, Position deltaPosition)
            : base(ClientPlayPacketType.EntityPositionAndRotation)
        {
            WriteVarInt32(entity.EntityId);
            WriteInt16((short)deltaPosition.X);
            WriteInt16((short)deltaPosition.Y);
            WriteInt16((short)deltaPosition.Z);
            WriteAngle(entity.Angle);
            WriteAngle(entity.HeadAngle);
            WriteBoolean(entity.IsOnGround);
        }
    }
}
