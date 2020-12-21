using Redstone.Abstractions.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redstone.Server
{
    public class WorldMap : IWorldMap
    {
        private readonly List<IRegion> _regions;

        public IEnumerable<IRegion> Regions => _regions;

        public string Name { get; }

        public WorldMap(string worldName)
        {
            Name = worldName;
            _regions = new List<IRegion>();
        }

        public IRegion AddRegion(int x, int z)
        {
            if (ContainsRegion(x, z))
            {
                throw new InvalidOperationException($"Region {x}/{z} already exists.");
            }

            var region = new Region(x, z);

            _regions.Add(region);

            return region;
        }

        public IRegion GetRegion(int x, int z)
        {
            return _regions.FirstOrDefault(region => region.X == x && region.Z == z);
        }

        public bool ContainsRegion(int x, int z) => _regions.Any(region => region.X == x && region.Z == z);
    }

    public class Region : IRegion
    {
        public const int Size = 512;
        public const int ChunkAmount = Size / Chunk.Size;
        
        private readonly List<IChunk> _chunks;

        public int X { get; }

        public int Z { get; }

        public IEnumerable<IChunk> Chunks => _chunks;

        public Region(int x, int z)
        {
            X = x;
            Z = z;
            _chunks = new List<IChunk>();
        }

        public IChunk GetChunk(int x, int z)
        {
            throw new NotImplementedException();
        }
    }

    public class Chunk : IChunk
    {
        public const int Size = 16;
        public const int Height = 255; // TODO: set this value according to dimesion settings.

        private readonly List<IChunkSection> _chunkSections;

        public int X { get; }

        public int Z { get; }

        public IEnumerable<IChunkSection> Sections => _chunkSections;

        public Chunk(int x, int z)
        {
            X = x;
            Z = z;

        }

        public IChunkSection GetSection(int sectionIndex)
        {
            throw new NotImplementedException();
        }
    }

    public class ChunkSection : IChunkSection
    {
        public int Index { get; }

        public ChunkSection(int index)
        {

        }
    }
}
