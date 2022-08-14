using Redstone.Abstractions.Entities;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client;

public class EntityPositionPacket : MinecraftPacket
{
    public EntityPositionPacket(IEntity entity, Position deltaPosition)
        : base(ClientPlayPacketType.EntityPosition)
    {
        WriteVarInt32(entity.EntityId);
        WriteInt16((short)deltaPosition.X);
        WriteInt16((short)deltaPosition.Y);
        WriteInt16((short)deltaPosition.Z);
        WriteBoolean(entity.IsOnGround);
    }
}
