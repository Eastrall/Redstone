using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Collections;
using Redstone.NBT;
using Redstone.NBT.Tags;
using Redstone.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Redstone.Server.World
{
    public class Chunk : IChunk
    {
        public const int Size = 16;
        public const int Height = 255; // TODO: set this value according to dimesion settings.
        public const int ChunkSectionAmount = 16;

        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<ChunkSection> _chunkSections;

        private readonly CompactedLongArray _heightmap;
        private readonly CompactedLongArray _oceanFloorHeightmap;
        private readonly CompactedLongArray _worldSurfaceHeightmap;

        public bool IsDirty => _chunkSections.Any(x => x.IsDirty);

        public int X { get; }

        public int Z { get; }

        public int BlockAmount => _chunkSections.Sum(x => x.GetBlockAmount());

        public IEnumerable<long> Heightmap => _heightmap.Storage;

        public IEnumerable<long> WorldSurfaceHeightmap => _worldSurfaceHeightmap.Storage;

        public IEnumerable<IChunkSection> Sections => _chunkSections;

        public Chunk(int x, int z, IServiceProvider serviceProvider)
        {
            X = x;
            Z = z;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _chunkSections = Enumerable.Range(0, ChunkSectionAmount).Select(index => new ChunkSection(this, index, _serviceProvider)).ToList();

            _heightmap = new CompactedLongArray(9, 256);
            _oceanFloorHeightmap = new CompactedLongArray(9, 256);
            _worldSurfaceHeightmap = new CompactedLongArray(9, 256);
        }

        public void GenerateHeightMap()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int z = 0; z < Size; z++)
                {
                    for (int y = Height; y >= 0; y--)
                    {
                        var block = GetBlock(x, y, z);

                        if (block.IsAir)
                            continue;

                        _heightmap[x + z * Size] = y;
                        break;
                    }
                }
            }
        }

        public IBlock GetBlock(int x, int y, int z)
        {
            if (y < 0)
            {
                throw new InvalidOperationException($"Cannot get a block with a negative Y value.");
            }

            if (y > Height)
            {
                throw new InvalidOperationException($"Cannot get a block with a Y value higher than {Height}.");
            }

            int sectionIndex = GetSectionIndex(y);
            IChunkSection section = GetSection(sectionIndex);

            return section.GetBlock(x, y % Size, z);
        }

        public IChunkSection GetSection(int sectionIndex)
        {
            IChunkSection section = _chunkSections.ElementAtOrDefault(sectionIndex);

            if (section is null)
            {
                throw new InvalidOperationException($"Failed to get section with index '{sectionIndex}'.");
            }

            return section;
        }

        public IBlock SetBlock(BlockType blockType, int x, int y, int z)
        {
            if (x < 0 || x >= Size)
            {
                throw new InvalidOperationException("X position is out of chunk bounds.");
            }

            if (z < 0 || z >= Size)
            {
                throw new InvalidOperationException("Z position is out of chunk bounds.");
            }

            IChunkSection section = GetChunkSection(y);
            IBlock block = section.SetBlock(blockType, x, y % Size, z);

            // TODO: improve heightmap generation when placing a block.
            GenerateHeightMap();

            return block;
        }

        [ExcludeFromCodeCoverage]
        public void Serialize(IMinecraftPacket packet, bool fullChunk = false)
        {
            packet.WriteInt32(X); // Chunk X
            packet.WriteInt32(Z); // Chunk Z
            packet.WriteBoolean(fullChunk); // full chunk

            int mask = 0;

            // if full chunk
            using var chunkStream = new MinecraftPacket();
            for (int i = 0; i < Sections.Count(); i++)
            {
                IChunkSection section = Sections.ElementAt(i);

                if (fullChunk || section.IsDirty)
                {
                    mask |= 1 << i;
                    section.Serialize(chunkStream);
                }
            }

            packet.WriteVarInt32(mask);

            // Heightmap serialization
            var heightmapCompound = new NbtCompound("")
            {
                new NbtLongArray("MOTION_BLOCKING", Heightmap.ToArray()),
                new NbtLongArray("WORLD_SURFACE", WorldSurfaceHeightmap.ToArray())
            };
            var nbtFile = new NbtFile(heightmapCompound);


            //var writer = new NbtWriter(this, "");
            //writer.WriteLongArray("MOTION_BLOCKING", Heightmap.ToArray());
            ////writer.WriteLongArray("OCEAN_FLOOR", chunk.Heightmaps[HeightmapType.OceanFloor].data.Storage.Cast<long>().ToArray());
            //writer.WriteLongArray("WORLD_SURFACE", WorldSurfaceHeightmap.ToArray());
            //writer.EndCompound();
            //writer.Finish();

            packet.WriteBytes(nbtFile.GetBuffer());

            // Biomes
            if (fullChunk)
            {
                packet.WriteVarInt32(1024);

                for (int i = 0; i < 1024; i++)
                {
                    packet.WriteVarInt32(0);
                }
            }

            chunkStream.Position = 0;

            packet.WriteVarInt32((int)chunkStream.Length);
            packet.WriteBytes(chunkStream.BaseBuffer);

            packet.WriteVarInt32(0); // block count
            // TODO: foreach block in blocks in chunk as NBT
        }

        private IChunkSection GetChunkSection(int yCoordinate)
        {
            if (yCoordinate < 0)
            {
                throw new InvalidOperationException($"Cannot get a block with a negative Y value.");
            }

            if (yCoordinate > Height)
            {
                throw new InvalidOperationException($"Cannot get a block with a height value higher than '{Height}'.");
            }

            int sectionIndex = GetSectionIndex(yCoordinate);

            return GetSection(sectionIndex);
        }

        private static int GetSectionIndex(int y) => y / Size;
    }
}
