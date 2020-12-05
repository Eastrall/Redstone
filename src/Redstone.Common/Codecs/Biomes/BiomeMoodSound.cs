using System.Text.Json.Serialization;

namespace Redstone.Common.Codecs.Biomes
{
    public class BiomeMoodSound
    {
        [JsonPropertyName("tick_delay")]
        public int TickDelay { get; set; }

        public double Offset { get; set; }

        public string Sound { get; set; }

        [JsonPropertyName("block_search_extent")]
        public int BlockSearchExtent { get; set; }
    }
}
