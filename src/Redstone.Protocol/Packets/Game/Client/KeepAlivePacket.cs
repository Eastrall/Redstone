namespace Redstone.Protocol.Packets.Game.Client;

public class KeepAlivePacket : MinecraftPacket
{
    public KeepAlivePacket(long keepAliveId)
        : base(ClientPlayPacketType.KeepAlive)
    {
        WriteInt64(keepAliveId);
    }
}
