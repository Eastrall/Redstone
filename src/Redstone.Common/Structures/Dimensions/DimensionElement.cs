using Redstone.NBT;
using Redstone.NBT.Serialization;
using System.Text.Json.Serialization;

namespace Redstone.Common.Structures.Dimensions;

/// <summary>
/// Describes the dimension element data structure.
/// </summary>
public class DimensionElement
{
    /// <summary>
    /// Gets or sets a boolean value that indicates if whether piglins shake and transform to zombified piglins.
    /// </summary>
    [JsonPropertyName("piglin_safe")]
    [NbtElement(NbtTagType.Byte, "piglin_safe")]
    public bool PiglinSafe { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that when false, compasses spin randomly. When true, nether portals can spawn zombified piglins.
    /// </summary>
    [JsonPropertyName("natural")]
    [NbtElement(NbtTagType.Byte, "natural")]
    public bool IsNatural { get; set; }

    /// <summary>
    /// Gets or sets how much light the dimension has.
    /// </summary>
    [JsonPropertyName("ambient_light")]
    [NbtElement(NbtTagType.Float, "ambient_light")]
    public float AmbientLight { get; set; }

    /// <summary>
    /// Gets or sets the time of the day is the specified value.
    /// </summary>
    [JsonPropertyName("fixed_time")]
    [NbtElement(NbtTagType.Long, "fixed_time")]
    public long FixedTime { get; set; }

    /// <summary>
    /// Gets or sets the resource location defining what block tag to use for infiniburn.
    /// </summary>
    [NbtElement(NbtTagType.String, "infiniburn")]
    public string Infiniburn { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if players can charge and use respawn anchors.
    /// </summary>
    [JsonPropertyName("respawn_anchor_works")]
    [NbtElement(NbtTagType.Byte, "respawn_anchor_works")]
    public bool RespawnAnchorWorks { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether players can charge and use respawn anchors.	
    /// </summary>
    [JsonPropertyName("has_skylight")]
    [NbtElement(NbtTagType.Byte, "has_skylight")]
    public bool HasSkylight { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the dimension has skylight access or not.
    /// </summary>
    [JsonPropertyName("bed_works")]
    [NbtElement(NbtTagType.Byte, "bed_works")]
    public bool BedWorks { get; set; }

    /// <summary>
    /// Gets or sets the effect files.
    /// </summary>
    [NbtElement(NbtTagType.String, "effects")]
    public string Effects { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether players with the Bad Omen effect can cause a raid.
    /// </summary>
    [JsonPropertyName("has_raids")]
    [NbtElement(NbtTagType.Byte, "has_raids")]
    public bool HasRaids { get; set; }

    /// <summary>
    /// Gets or sets the maximum height to which chorus fruits and nether portals can bring players within this dimension.	
    /// </summary>
    [JsonPropertyName("logical_height")]
    [NbtElement(NbtTagType.Int, "logical_height")]
    public int LogicalHeight { get; set; }

    /// <summary>
    /// Gets or sets the multiplier applied to coordinates when traveling to the dimension.
    /// </summary>
    [JsonPropertyName("coordinate_scale")]
    [NbtElement(NbtTagType.Float, "coordinate_scale")]
    public float CoordinateScale { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the dimensions behaves like the nether (water evaporates and sponges dry) or not. Also causes lava to spread thinner.
    /// </summary>
    [JsonPropertyName("ultrawarm")]
    [NbtElement(NbtTagType.Byte, "ultrawarm")]
    public bool IsUltrawarm { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the dimension has a bedrock ceiling or not. When true, causes lava to spread faster.
    /// </summary>
    [JsonPropertyName("has_ceiling")]
    [NbtElement(NbtTagType.Byte, "has_ceiling")]
    public bool HasCeiling { get; set; }
}
