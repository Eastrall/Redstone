namespace Redstone.Common.Codecs
{
    /// <summary>
    /// Describes the dimension codec.
    /// </summary>
    public class DimensionCodec
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
        public DimensionCodecElement Element { get; set; }
    }
}
