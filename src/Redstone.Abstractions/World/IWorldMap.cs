using System;
using System.Collections.Generic;
using System.Text;

namespace Redstone.Abstractions.World
{
    public interface IWorldMap
    {
        IEnumerable<IRegion> Regions { get; }

        string Name { get; }

        IRegion AddRegion(int x, int z);
        
        IRegion GetRegion(int x, int z);

        bool ContainsRegion(int x, int z);
    }
}
