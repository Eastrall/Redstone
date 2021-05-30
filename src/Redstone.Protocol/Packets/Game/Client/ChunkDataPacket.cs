using Redstone.Abstractions.World;
using Redstone.NBT;
using System.Linq;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class ChunkDataPacket : MinecraftPacket
    {
        public ChunkDataPacket(IChunk chunk, bool serializeFullChunk = false)
            : base(ClientPlayPacketType.ChunkData)
        {
            WriteInt32(chunk.X); // Chunk X
            WriteInt32(chunk.Z); // Chunk Z
            WriteBoolean(serializeFullChunk); // full chunk

            int mask = 0;

            // if full chunk
            using var chunkStream = new MinecraftPacket();
            for (int i = 0; i < chunk.Sections.Count(); i++)
            {
                IChunkSection section = chunk.Sections.ElementAt(i);

                if (serializeFullChunk || section.IsDirty)
                {
                    mask |= 1 << i;
                    section.Serialize(chunkStream);
                }
            }

            WriteVarInt32(mask);

            //// Heightmap serialization
            ////var heightmapCompound = new NbtCompound("")
            ////{
            ////    new NbtLongArray("MOTION_BLOCKING", chunk.Heightmap.ToArray()),
            ////    new NbtLongArray("WORLD_SURFACE", chunk.WorldSurfaceHeightmap.ToArray())
            ////};
            ////var nbtFile = new NbtFile(heightmapCompound);


            var writer = new NbtWriter(this, "");
            writer.WriteLongArray("MOTION_BLOCKING", chunk.Heightmap.ToArray());
            //writer.WriteLongArray("OCEAN_FLOOR", chunk.Heightmaps[HeightmapType.OceanFloor].data.Storage.Cast<long>().ToArray());
            writer.WriteLongArray("WORLD_SURFACE", chunk.WorldSurfaceHeightmap.ToArray());
            writer.EndCompound();
            writer.Finish();

            //packet.WriteBytes(nbtFile.GetBuffer());

            // Biomes
            if (serializeFullChunk)
            {
                WriteVarInt32(1024);
                for (int i = 0; i < 1024; i++)
                {
                    WriteVarInt32(0);
                }
            }

            chunkStream.Position = 0;

            WriteVarInt32((int)chunkStream.Length);
            WriteBytes(chunkStream.BaseBuffer);

            WriteVarInt32(0); // block count
            // TODO: foreach block in blocks in chunk as NBT
        }
    }
}
