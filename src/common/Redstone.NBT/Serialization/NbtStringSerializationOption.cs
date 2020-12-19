namespace Redstone.NBT.Serialization
{
    /// <summary>
    /// Provides different serialization types for NBT string tags.
    /// </summary>
    public enum NbtStringSerializationOption
    {
        /// <summary>
        /// The default option. It serializes the string as it is.
        /// </summary>
        Default,

        /// <summary>
        /// Serializes the string in lower case mode.
        /// </summary>
        Lowercase,

        /// <summary>
        /// Serializes the string in upper case mode.
        /// </summary>
        Uppercase,
    }
}
