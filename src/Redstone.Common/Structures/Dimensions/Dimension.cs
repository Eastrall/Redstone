using Redstone.NBT;
using Redstone.NBT.Serialization;
using System.Diagnostics;

namespace Redstone.Common.Structures.Dimensions;

/// <summary>
/// Describes the dimension data structure.
/// </summary>
[DebuggerDisplay("{Name} (Id = {Id})")]
public class Dimension
{
    /// <summary>
    /// Gets or sets the dimension id.
    /// </summary>
    [NbtElement(NbtTagType.Int, "id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the dimension name.
    /// </summary>
    [NbtElement(NbtTagType.String, "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the dimension element.
    /// </summary>
    [NbtElement(NbtTagType.Compound, "element")]
    public DimensionElement Element { get; set; }
}
