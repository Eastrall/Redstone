using Redstone.NBT;
using Redstone.NBT.Serialization;
using System.Text.Json.Serialization;

namespace Redstone.Common.Structures.Biomes;

public class BiomeMoodSound
{
    [JsonPropertyName("tick_delay")]
    [NbtElement(NbtTagType.Int, "tick_delay")]
    public int TickDelay { get; set; }

    [NbtElement(NbtTagType.Double, "offset")]
    public double Offset { get; set; }

    [NbtElement(NbtTagType.String, "sound")]
    public string Sound { get; set; }

    [JsonPropertyName("block_search_extent")]
    [NbtElement(NbtTagType.Int, "block_search_extent")]
    public int BlockSearchExtent { get; set; }
}
