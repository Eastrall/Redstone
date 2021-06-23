using Redstone.Abstractions.World;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class ChunkDataPacket : MinecraftPacket
    {
        public ChunkDataPacket(IChunk chunk, bool serializeFullChunk = false)
            : base(ClientPlayPacketType.ChunkData)
        {
            chunk.Serialize(this, serializeFullChunk);
        }
    }
}
