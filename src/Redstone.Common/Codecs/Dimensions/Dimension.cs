using Redstone.NBT;
using Redstone.NBT.Attributes;
using System.Diagnostics;

namespace Redstone.Common.Codecs.Dimensions
{
    /// <summary>
    /// Describes the dimension data structure.
    /// </summary>
    [DebuggerDisplay("{Name} (Id = {Id})")]
    public class Dimension
    {
        /// <summary>
        /// Gets or sets the biome id.
        /// </summary>
        [NbtElement(NbtTagType.Int, "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the biome dimension name.
        /// </summary>
        [NbtElement(NbtTagType.String, "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dimension codec element.
        /// </summary>
        [NbtElement(NbtTagType.Compound, "element")]
        public DimensionElement Element { get; set; }
    }
}
