namespace Redstone.Protocol.Packets.Game.Client
{
    public class PlayerPositionAndLookPacket : MinecraftPacket
    {
        public PlayerPositionAndLookPacket()
            : base(ClientPlayPacketType.PlayerPositionAndLook)
        {
        }
    }
}
