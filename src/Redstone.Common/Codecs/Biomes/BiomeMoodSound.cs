using System.Text.Json.Serialization;

namespace Redstone.Common.Codecs.Biomes
{
    public class BiomeMoodSound
    {
        [JsonPropertyName("tick_delay")]
        public uint TickDelay { get; set; }

        public uint Offset { get; set; }

        public string Sound { get; set; }

        [JsonPropertyName("block_search_extent")]
        public int BlockSearchExtent { get; set; }
    }
}
