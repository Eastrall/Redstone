using Redstone.Abstractions.World;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IRegion GetRegion(int x, int z) => _regions.FirstOrDefault(region => region.X == x && region.Z == z);

        public bool ContainsRegion(int x, int z) => _regions.Any(region => region.X == x && region.Z == z);
    }
}
