using Redstone.NBT.Attributes;
using Redstone.NBT.Tags;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Redstone.NBT.Serialization
{
    public static class NbtSerializer
    {
        public static NbtCompound SerializeCompound<TObject>(TObject @object) where TObject : class, new()
        {
            IEnumerable<NbtObjectDescriptor> nbtProperties = LoadObjectMetadata(@object);

            var nbtCompound = new NbtCompound();

            foreach (NbtObjectDescriptor nbtProperty in nbtProperties)
            {
                NbtTag tag = null;

                if (nbtProperty.Type == NbtTagType.Compound || nbtProperty.Type == NbtTagType.List)
                {
                    // Recursion
                }
                else
                {
                    tag = CreateTag(nbtProperty.Type, nbtProperty.Name, nbtProperty.Property.GetValue(@object));
                }

                if (tag is not null)
                {
                    nbtCompound.Add(tag);
                }
            }

            return nbtCompound;
        }

        public static TObject DeserializeCompound<TObject>(NbtCompound nbtCompound) where TObject : class, new()
        {
            return default;
        }

        private static IEnumerable<NbtObjectDescriptor> LoadObjectMetadata<TObject>(TObject @object)
        {
            return from x in typeof(TObject).GetProperties()
                   let attribute = x.GetCustomAttribute<NbtElementAttribute>()
                   where attribute != null
                   select new NbtObjectDescriptor(attribute.Type, attribute.Name, x);
        }

        private static NbtTag CreateTag(NbtTagType type, string name, object value)
        {
            return type switch
            {
                NbtTagType.Int => new NbtInt(name, Convert.ToInt32(value)),
                NbtTagType.Float => new NbtFloat(name, Convert.ToSingle(value)),
                NbtTagType.String => new NbtString(name, value?.ToString()),
            };
        }
    }

    [DebuggerDisplay("{Type} {Name}")]
    internal class NbtObjectDescriptor
    {
        public NbtTagType Type { get; }

        public string Name { get; }

        public PropertyInfo Property { get; }

        public NbtObjectDescriptor(NbtTagType type, string name, PropertyInfo property)
        {
            Type = type;
            Name = name;
            Property = property;
        }
    }
}
