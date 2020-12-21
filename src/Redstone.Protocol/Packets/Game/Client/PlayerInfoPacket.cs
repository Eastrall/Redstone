namespace Redstone.Protocol.Packets.Game.Client
{
    public class PlayerInfoPacket : MinecraftPacket
    {
        public PlayerInfoPacket()
            : base(ClientPlayPacketType.PlayerInfo)
        {
        }
    }
}
