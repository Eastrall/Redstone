namespace Redstone.Protocol.Packets.Game.Client
{
    public class ChunkDataPacket : MinecraftPacket
    {
        public ChunkDataPacket()
            : base(ClientPlayPacketType.ChunkData)
        {
        }
    }
}
