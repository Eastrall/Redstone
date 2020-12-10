using Redstone.NBT;
using Redstone.NBT.Serialization;

namespace Redstone.Common.Codecs.Biomes
{
    public class BiomeElement
    {
        [NbtElement(NbtTagType.String, "precipitation", StringSerialization = NbtStringSerializationOption.Lowercase)]
        public PrecipitationType Precipitation { get; set; }

        [NbtElement(NbtTagType.Float, "depth")]
        public float Depth { get; set; }

        [NbtElement(NbtTagType.Float, "temperature")]
        public float Temperature { get; set; }

        [NbtElement(NbtTagType.Float, "scale")]
        public float Scale { get; set; }

        [NbtElement(NbtTagType.Float, "downfall")]
        public float DownFall { get; set; }

        [NbtElement(NbtTagType.String, "category", StringSerialization = NbtStringSerializationOption.Lowercase)]
        public BiomeCategoryType Category { get; set; }

        [NbtElement(NbtTagType.Compound, "effects")]
        public BiomeEffect Effects { get; set; }
    }
}
