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

        public int X { get; }

        public int Z { get; }

        public IEnumerable<IChunkSection> Sections => _chunkSections;

        public Chunk(int x, int z)
        {
            X = x;
            Z = z;
            _chunkSections = Enumerable.Range(0, ChunkSectionAmount).Select(index => new ChunkSection(index)).ToList();
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
    }
}
