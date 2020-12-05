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
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the biome dimension name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dimension codec element.
        /// </summary>
        public DimensionElement Element { get; set; }
    }
}
