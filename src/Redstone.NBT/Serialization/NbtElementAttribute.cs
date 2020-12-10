using System;

namespace Redstone.NBT.Serialization
{
    /// <summary>
    /// Data annotation describing a NBT element.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NbtElementAttribute : Attribute
    {
        /// <summary>
        /// Gets the Nbt element type.
        /// </summary>
        public NbtTagType Type { get; }

        /// <summary>
        /// Gets the NBT compound name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the Nbt string serialization option.
        /// </summary>
        public NbtStringSerializationOption StringSerialization { get; set; } = NbtStringSerializationOption.Default;

        /// <summary>
        /// Creates a new <see cref="NbtElementAttribute"/> instance.
        /// </summary>
        /// <param name="type">NBT tag type.</param>
        /// <param name="name">NBT tag name.</param>
        public NbtElementAttribute(NbtTagType type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
