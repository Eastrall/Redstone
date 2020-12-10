using System.Diagnostics;
using System.Reflection;

namespace Redstone.NBT.Serialization
{
    /// <summary>
    /// Provides a data structure that describes a serialized nbt element.
    /// </summary>
    [DebuggerDisplay("{Type} {Name}")]
    internal class NbtObjectDescriptor
    {
        /// <summary>
        /// Gets the NBT tag type.
        /// </summary>
        public NbtTagType Type { get; }

        /// <summary>
        /// Gets the NBT element name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the attached property.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Creates a new <see cref="NbtObjectDescriptor"/> instance.
        /// </summary>
        /// <param name="type">Nbt tag type.</param>
        /// <param name="name">Nbt tag name.</param>
        /// <param name="property">Object property information.</param>
        public NbtObjectDescriptor(NbtTagType type, string name, PropertyInfo property)
        {
            Type = type;
            Name = name;
            Property = property;
        }
    }
}
