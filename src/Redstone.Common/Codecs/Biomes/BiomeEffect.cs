using System.Text.Json.Serialization;

namespace Redstone.Common.Codecs.Biomes
{
    public class BiomeEffect
    {
        [JsonPropertyName("sky_color")]
        public uint SkyColor { get; set; }

        [JsonPropertyName("water_fog_color")]
        public uint WaterFogColor { get; set; }

        [JsonPropertyName("fog_color")]
        public uint FogColor { get; set; }

        [JsonPropertyName("water_color")]
        public uint WaterColor { get; set; }

        [JsonPropertyName("mood_sound")]
        public BiomeMoodSound MoodSound { get; set; }
    }
}
