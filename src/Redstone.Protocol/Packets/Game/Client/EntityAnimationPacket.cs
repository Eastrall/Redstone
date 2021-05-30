using Redstone.Abstractions.Entities;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class EntityAnimationPacket : MinecraftPacket
    {
        public EntityAnimationPacket(IEntity entity, AnimationType animationType)
            : base(ClientPlayPacketType.EntityAnimation)
        {
            WriteVarInt32(entity.EntityId);
            WriteByte((byte)animationType);
        }
    }
}
