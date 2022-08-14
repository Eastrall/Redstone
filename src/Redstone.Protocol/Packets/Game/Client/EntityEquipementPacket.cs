using Redstone.Abstractions.Components;
using Redstone.Abstractions.Entities;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client;

public class EntityEquipementPacket : MinecraftPacket
{
    public EntityEquipementPacket(IEntity entity, EquipementSlotType equipementSlot, IItemSlot item)
        : base(ClientPlayPacketType.EntityEquipement)
    {
        WriteVarInt32(entity.EntityId);
        WriteByte((byte)equipementSlot);
        WriteBoolean(item.HasItem);

        if (item.HasItem)
        {
            WriteVarInt32(item.ItemId.Value);
            WriteByte(item.ItemCount);
            WriteByte(0);
        }
    }
}
