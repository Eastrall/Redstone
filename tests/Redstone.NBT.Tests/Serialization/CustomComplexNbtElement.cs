using Redstone.NBT.Serialization;

namespace Redstone.NBT.Tests.Serialization
{
    internal class CustomComplexNbtElement
    {
        [NbtElement(NbtTagType.Int, "integer_value")]
        public int IntegerValue { get; set; }

        [NbtElement(NbtTagType.Compound, "flat_element_compound")]
        public CustomFlatNbtElement FlatElement { get; set; }
    }
}
