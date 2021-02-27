using Redstone.Abstractions.World;
using Redstone.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server.World
{
    public class Chunk : IChunk
    {
        public const int Size = 16;
        public const int Height = 255; // TODO: set this value according to dimesion settings.
        public const int ChunkSectionAmount = 16;

        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IChunkSection> _chunkSections;

        private readonly CompactedLongArray _heightmap;
        private readonly CompactedLongArray _oceanFloorHeightmap;
        private readonly CompactedLongArray _worldSurfaceHeightmap;

        public int X { get; }

        public int Z { get; }

        public IEnumerable<long> Heightmap => _heightmap.Storage;

        public IEnumerable<long> WorldSurfaceHeightmap => _worldSurfaceHeightmap.Storage;

        public IEnumerable<IChunkSection> Sections => _chunkSections;

        public Chunk(int x, int z, IServiceProvider serviceProvider)
        {
            X = x;
            Z = z;
            _serviceProvider = serviceProvider;
            _chunkSections = Enumerable.Range(0, ChunkSectionAmount).Select(index => new ChunkSection(index, _serviceProvider)).ToList();

            _heightmap = new CompactedLongArray(9, 256);
            _oceanFloorHeightmap = new CompactedLongArray(9, 256);
            _worldSurfaceHeightmap = new CompactedLongArray(9, 256);
        }

        public void GenerateHeightMap()
        {
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 255; y >= 0; y--)
                    {
                        var block = GetBlock(x, y, z);

                        if (block.IsAir)
                            continue;

                        _heightmap[x + z * 16] = y;
                        break;
                    }
                }
            }
        }

        public IBlock GetBlock(int x, int y, int z)
        {
            int sectionIndex = y / 16;
            IChunkSection section = _chunkSections.ElementAtOrDefault(sectionIndex); // TODO: check null

            return section.GetBlock(x, y % 16, z);
        }

        public IChunkSection GetSection(int sectionIndex)
        {
            return _chunkSections.ElementAtOrDefault(sectionIndex); // TODO: check null
        }

        public void SetBlock(IBlock block, int x, int y, int z)
        {
            int sectionIndex = y / 16;
            IChunkSection section = _chunkSections.ElementAtOrDefault(sectionIndex); // TODO: check null

            section.SetBlock(block, x, y % 16, z);
        }
    }
}
