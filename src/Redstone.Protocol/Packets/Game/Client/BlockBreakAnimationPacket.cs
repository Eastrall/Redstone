using Redstone.Abstractions.Entities;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class BlockBreakAnimationPacket : MinecraftPacket
    {
        public BlockBreakAnimationPacket(IPlayer player, Position position, byte displayStage = byte.MaxValue)
            : base(ClientPlayPacketType.BlockBreakAnimation)
        {
            WriteVarInt32(player.EntityId);
            WritePosition(position);
            WriteByte(displayStage);
        }
    }
}
