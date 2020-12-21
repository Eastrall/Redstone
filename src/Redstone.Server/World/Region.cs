using Redstone.Abstractions.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server
{
    public class Region : IRegion
    {
        public const int Size = 512;
        public const int ChunkAmount = Size / Chunk.Size;
        
        private readonly IChunk[] _chunks;

        public int X { get; }

        public int Z { get; }

        public IEnumerable<IChunk> Chunks => _chunks;

        public Region(int x, int z)
        {
            X = x;
            Z = z;
            _chunks = Enumerable.Repeat(default(IChunk), ChunkAmount * ChunkAmount).ToArray();
        }

        public IChunk AddChunk(int x, int z)
        {
            if (ContainsChunk(x, z))
            {
                throw new InvalidOperationException($"Failed to add chunk at {x}/{z}. Chunk already exists.");
            }

            var chunk = new Chunk(x, z);

            _chunks[GetChunkIndex(x, z)] = chunk;

            return chunk;
        }
        public bool ContainsChunk(int x, int z) => GetChunk(x, z) != null;

        public IChunk GetChunk(int x, int z) => _chunks[GetChunkIndex(x, z)];

        private int GetChunkIndex(int x, int z) => z + (ChunkAmount * x);
    }
}
