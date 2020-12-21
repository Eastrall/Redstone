using Redstone.NBT;
using Redstone.NBT.Serialization;
using System.Text.Json.Serialization;

namespace Redstone.Common.Structures.Biomes
{
    public class BiomeEffect
    {
        [JsonPropertyName("sky_color")]
        [NbtElement(NbtTagType.Float, "sky_color")]
        public uint SkyColor { get; set; }

        [JsonPropertyName("water_fog_color")]
        [NbtElement(NbtTagType.Float, "water_fog_color")]
        public uint WaterFogColor { get; set; }

        [JsonPropertyName("fog_color")]
        [NbtElement(NbtTagType.Float, "fog_color")]
        public uint FogColor { get; set; }

        [JsonPropertyName("water_color")]
        [NbtElement(NbtTagType.Float, "water_color")]
        public uint WaterColor { get; set; }

        [JsonPropertyName("mood_sound")]
        [NbtElement(NbtTagType.Compound, "mood_sound")]
        public BiomeMoodSound MoodSound { get; set; }
    }
}
