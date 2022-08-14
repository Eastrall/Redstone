using Redstone.Common;

namespace Redstone.Protocol.Packets.Game.Client;

public class SpawnPositionPacket : MinecraftPacket
{
    public SpawnPositionPacket(Position position)
        : base(ClientPlayPacketType.SpawnPosition)
    {
        WritePosition(position);
    }
}
