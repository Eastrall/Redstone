using Redstone.Abstractions.World;
using Redstone.Common;
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

        public bool IsDirty => _chunkSections.Any(x => x.IsDirty);

        public int X { get; }

        public int Z { get; }

        public IEnumerable<long> Heightmap => _heightmap.Storage;

        public IEnumerable<long> WorldSurfaceHeightmap => _worldSurfaceHeightmap.Storage;

        public IEnumerable<IChunkSection> Sections => _chunkSections;

        public Chunk(int x, int z, IServiceProvider serviceProvider)
        {
            X = x;
            Z = z;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _chunkSections = Enumerable.Range(0, ChunkSectionAmount).Select(index => new ChunkSection(index, _serviceProvider)).ToList();

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

        public void SetBlock(BlockType blockType, int x, int y, int z)
        {
            GetChunkSection(y).SetBlock(blockType, x, y % Size, z);

            // TODO: improve heightmap generation when placing a block.
            GenerateHeightMap();
        }

        public void SetBlock(IBlock block, int x, int y, int z)
        {
            GetChunkSection(y).SetBlock(block, x, y % Size, z);
        }

        private IChunkSection GetChunkSection(int yCoordinate)
        {
            if (yCoordinate < 0)
            {
                throw new InvalidOperationException($"Cannot get a block with a negative Y value.");
            }

            int sectionIndex = GetSectionIndex(yCoordinate);

            return GetSection(sectionIndex);
        }

        private static int GetSectionIndex(int y) => y / Size;
    }
}
