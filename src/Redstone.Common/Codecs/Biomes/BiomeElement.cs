namespace Redstone.Common.Codecs.Biomes
{
    public class BiomeElement
    {
        public PrecipitationType Precipitation { get; set; }

        public float Depth { get; set; }

        public float Temperature { get; set; }

        public float Scale { get; set; }

        public float DownFall { get; set; }

        public BiomeCategoryType Category { get; set; }

        public BiomeEffect Effects { get; set; }
    }
}
