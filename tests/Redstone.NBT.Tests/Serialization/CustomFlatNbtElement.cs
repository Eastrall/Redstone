using Redstone.NBT.Serialization;

namespace Redstone.NBT.Tests.Serialization
{
    internal class CustomFlatNbtElement
    {
        [NbtElement(NbtTagType.Byte, "byte_value")]
        public byte ByteValue { get; set; }

        [NbtElement(NbtTagType.Int, "integer_value")]
        public int IntegerValue { get; set; }

        [NbtElement(NbtTagType.Short, "short_value")]
        public short ShortValue { get; set; }

        [NbtElement(NbtTagType.Float, "float_value")]
        public float FloatValue { get; set; }

        [NbtElement(NbtTagType.Long, "long_value")]
        public long LongValue { get; set; }

        [NbtElement(NbtTagType.Double, "double_value")]
        public double DoubleValue { get; set; }

        [NbtElement(NbtTagType.String, "string_value")]
        public string StringValue { get; set; }
    }
}
