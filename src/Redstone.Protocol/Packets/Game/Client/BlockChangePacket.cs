using Redstone.Abstractions.World;
using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client;

public class BlockChangePacket : MinecraftPacket
{
    public BlockChangePacket(BlockType blockType, Position blockPosition)
        : base(ClientPlayPacketType.BlockChange)
    {
        WritePosition(blockPosition);
        WriteVarInt32((int)blockType);
    }
}
