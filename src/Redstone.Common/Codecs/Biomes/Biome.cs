using System;
using System.Collections.Generic;
using System.Text;

namespace Redstone.Common.Codecs.Biomes
{
    public class Biome
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Depth { get; set; }

        public float Temperature { get; set; }

        public float Scale { get; set; }

        public float DownFall { get; set; }

        public string Category { get; set; }

        public BiomeElement Element { get; set; }
    }
}
