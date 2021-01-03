using Redstone.Abstractions.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server
{
    public class Chunk : IChunk
    {
        public const int Size = 16;
        public const int Height = 255; // TODO: set this value according to dimesion settings.
        public static readonly int ChunkSectionAmount = Height / ChunkSection.Size;

        private readonly IEnumerable<IChunkSection> _chunkSections;
        private readonly long[] _heightmap;
        private readonly IServiceProvider _serviceProvider;

        public int X { get; }

        public int Z { get; }

        public IEnumerable<long> Heightmap => _heightmap;

        public IEnumerable<IChunkSection> Sections => _chunkSections;

        public Chunk(int x, int z, IServiceProvider serviceProvider)
        {
            X = x;
            Z = z;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _chunkSections = Enumerable.Range(0, ChunkSectionAmount).Select(index => new ChunkSection(index, _serviceProvider)).ToList();
            _heightmap = new long[Size * Size];
        }

        public IChunkSection GetSection(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= _chunkSections.Count())
            {
                throw new IndexOutOfRangeException("Chunk section index was out of range.");
            }

            return _chunkSections.ElementAt(sectionIndex);
        }

        public void GenerateHeightMap()
        {
            for (int i = _chunkSections.Count() - 1; i >= 0; i--)
            {
                IChunkSection section = _chunkSections.ElementAt(i);

                for (int x = 0; x < Size; x++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        for (int y = ChunkSection.Size - 1; y >= 0; y--)
                        {
                            IBlock block = section.GetBlock(x, y, z);

                            if (block.IsAir)
                            {
                                continue;
                            }

                            _heightmap[z + (x * Size)] = (long)block.Position.Y * (section.Index + 1);
                            break;
                        }
                    }
                }
            }
        }

        public void SetBlock(IBlock block, int x, int y, int z)
        {
            if (y < 0 || y >= Height)
            {
                throw new InvalidOperationException($"Cannot set block. Invalid Y position.");
            }

            int sectionIndex = y / ChunkSectionAmount;
            IChunkSection section = _chunkSections.ElementAt(sectionIndex);

            if (section is null)
            {
                throw new InvalidOperationException($"Section at {sectionIndex} is null.");
            }

            section.SetBlock(block, x, y % ChunkSectionAmount, z);
        }

        public IBlock GetBlock(int x, int y, int z)
        {
            throw new NotImplementedException();
        }
    }
}
