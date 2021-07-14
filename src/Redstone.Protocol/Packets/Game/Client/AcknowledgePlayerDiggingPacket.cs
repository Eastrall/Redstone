using Redstone.Abstractions.World;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class AcknowledgePlayerDiggingPacket : MinecraftPacket
    {
        public AcknowledgePlayerDiggingPacket(Position position, IBlock block, DiggingType diggingType, bool successful = true)
            : base(ClientPlayPacketType.AcknowledgePlayerDigging)
        {
            WritePosition(position);
            WriteVarInt32(block.State.Id);
            WriteVarInt32((int)diggingType);
            WriteBoolean(successful);
        }
    }
}
