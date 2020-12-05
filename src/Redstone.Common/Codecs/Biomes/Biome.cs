using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Redstone.Common.Codecs.Biomes
{
    [DebuggerDisplay("{Name} (Id = {Id})")]
    public class Biome
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public BiomeElement Element { get; set; }
    }
}
