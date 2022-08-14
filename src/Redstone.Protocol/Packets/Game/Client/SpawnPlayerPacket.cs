using Redstone.Abstractions.Entities;

namespace Redstone.Protocol.Packets.Game.Client;

public class SpawnPlayerPacket : MinecraftPacket
{
    public SpawnPlayerPacket(IPlayer player)
        : base(ClientPlayPacketType.SpawnPlayer)
    {
        WriteVarInt32(player.EntityId);
        WriteUUID(player.Id);
        WriteDouble(player.Position.X);
        WriteDouble(player.Position.Y);
        WriteDouble(player.Position.Z);
        WriteAngle(player.Angle);
        WriteAngle(player.HeadAngle);
    }
}
