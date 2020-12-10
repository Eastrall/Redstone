using Redstone.NBT.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redstone.NBT.Serialization
{
    /// <summary>
    /// Provides a mechanism to serrialize and deserialize Nbt compounds.
    /// </summary>
    public static class NbtSerializer
    {
        /// <summary>
        /// Serializes the given object into a <see cref="NbtCompound"/>.
        /// </summary>
        /// <typeparam name="TObject">Object type to serialize.</typeparam>
        /// <param name="object">Object to serialize.</param>
        /// <param name="compoundName">Optionnal compound name.</param>
        /// <returns>Given object serialized as a <see cref="NbtCompound"/>.</returns>
        public static NbtCompound SerializeCompound<TObject>(TObject @object, string compoundName = null) where TObject : class, new()
        {
            IEnumerable<NbtObjectDescriptor> nbtProperties = LoadObjectMetadata(@object);

            var nbtCompound = new NbtCompound(compoundName);

            foreach (NbtObjectDescriptor nbtProperty in nbtProperties)
            {
                NbtTag tag = null;
                object propertyValue = nbtProperty.Property.GetValue(@object);

                if (nbtProperty.Type == NbtTagType.Compound)
                {
                    tag = SerializeCompound(propertyValue, nbtProperty.Name);
                }
                else if (nbtProperty.Type == NbtTagType.List)
                {
                    // TODO
                }
                else
                {
                    tag = CreateTag(nbtProperty, propertyValue);
                }

                if (tag is not null)
                {
                    nbtCompound.Add(tag);
                }
            }

            return nbtCompound;
        }

        /// <summary>
        /// Deserializes a <see cref="NbtCompound"/> into a C# object.
        /// </summary>
        /// <typeparam name="TObject">Object type.</typeparam>
        /// <param name="nbtCompound">NBT Compound to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        public static TObject DeserializeCompound<TObject>(NbtCompound nbtCompound) where TObject : class, new()
        {
            return default;
        }

        /// <summary>
        /// Loads the object attributes metadata.
        /// </summary>
        /// <typeparam name="TObject">Object type.</typeparam>
        /// <param name="object">Object.</param>
        /// <returns>Collection containing the given object attribute metadata.</returns>
        private static IEnumerable<NbtObjectDescriptor> LoadObjectMetadata<TObject>(TObject @object)
        {
            if (@object is null)
            {
                return Enumerable.Empty<NbtObjectDescriptor>();
            }

            return from x in @object.GetType().GetProperties()
                   let attribute = x.GetCustomAttribute<NbtElementAttribute>()
                   where attribute != null
                   select new NbtObjectDescriptor(attribute.Type, attribute.Name, x)
                   {
                       StringSerializationOption = attribute.StringSerialization
                   };
        }

        /// <summary>
        /// Creates a new <see cref="NbtTag"/> based on the given type.
        /// </summary>
        /// <param name="nbtObjectProperties">Nbt element object scriptor.</param>
        /// <param name="value">Nbt element value.</param>
        /// <returns>NBT Tag.</returns>
        private static NbtTag CreateTag(NbtObjectDescriptor nbtObjectProperties, object value)
        {
            return nbtObjectProperties.Type switch
            {
                NbtTagType.Byte => new NbtByte(nbtObjectProperties.Name, Convert.ToByte(value)),
                NbtTagType.Short => new NbtShort(nbtObjectProperties.Name, Convert.ToInt16(value)),
                NbtTagType.Int => new NbtInt(nbtObjectProperties.Name, Convert.ToInt32(value)),
                NbtTagType.Float => new NbtFloat(nbtObjectProperties.Name, Convert.ToSingle(value)),
                NbtTagType.Long => new NbtLong(nbtObjectProperties.Name, Convert.ToInt64(value)),
                NbtTagType.Double => new NbtDouble(nbtObjectProperties.Name, Convert.ToDouble(value)),
                NbtTagType.String => CreateNbtString(nbtObjectProperties, value),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Creates a new <see cref="NbtString"/> from a <see cref="NbtObjectDescriptor"/>.
        /// </summary>
        /// <param name="nbtObjectProperties">Nbt element object scriptor.</param>
        /// <param name="value">Nbt element value.</param>
        /// <returns>NbtString</returns>
        private static NbtString CreateNbtString(NbtObjectDescriptor nbtObjectProperties, object value)
        {
            string stringValue = nbtObjectProperties.StringSerializationOption switch
            {
                NbtStringSerializationOption.Default => value?.ToString(),
                NbtStringSerializationOption.Lowercase => value?.ToString().ToLower(),
                NbtStringSerializationOption.Uppercase => value?.ToString().ToUpper(),
                _ => null
            };

            return new NbtString(nbtObjectProperties.Name, stringValue);
        }
    }
}
