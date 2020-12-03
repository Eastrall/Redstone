using Redstone.NBT.Tags;
using System;
using System.Globalization;
using Xunit;

namespace Redstone.NBT.Tests
{
    public class ShortcutTests
    {
        [Fact]
        public void NbtByteTest()
        {
            object dummy;
            NbtTag test = new NbtByte(250);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Equal(250, test.ByteValue);
            Assert.Equal((double)250, test.DoubleValue);
            Assert.Equal((float)250, test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Equal(250, test.IntValue);
            Assert.Equal(250L, test.LongValue);
            Assert.Equal(250, test.ShortValue);
            Assert.Equal("250", test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtByteArrayTest()
        {
            object dummy;
            byte[] bytes = { 1, 2, 3, 4, 5 };
            NbtTag test = new NbtByteArray(bytes);
            Assert.Equal(bytes, test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.DoubleValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtCompoundTest()
        {
            object dummy;
            NbtTag test = new NbtCompound("Derp");
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.DoubleValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.StringValue);
            Assert.False(test.HasValue);
        }


        [Fact]
        public void NbtDoubleTest()
        {
            object dummy;
            NbtTag test = new NbtDouble(0.4931287132182315);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Equal(0.4931287132182315, test.DoubleValue);
            Assert.Equal((float)0.4931287132182315, test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Equal((0.4931287132182315).ToString(CultureInfo.InvariantCulture), test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtFloatTest()
        {
            object dummy;
            NbtTag test = new NbtFloat(0.49823147f);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Equal((double)0.49823147f, test.DoubleValue);
            Assert.Equal(0.49823147f, test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Equal((0.49823147f).ToString(CultureInfo.InvariantCulture), test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtIntTest()
        {
            object dummy;
            NbtTag test = new NbtInt(2147483647);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Equal((double)2147483647, test.DoubleValue);
            Assert.Equal((float)2147483647, test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Equal(2147483647, test.IntValue);
            Assert.Equal(2147483647L, test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Equal("2147483647", test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtIntArrayTest()
        {
            object dummy;
            int[] ints = { 1111, 2222, 3333, 4444, 5555 };
            NbtTag test = new NbtIntArray(ints);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.DoubleValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.FloatValue);
            Assert.Equal(ints, test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtListTest()
        {
            object dummy;
            NbtTag test = new NbtList("Derp");
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.DoubleValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.StringValue);
            Assert.False(test.HasValue);
        }


        [Fact]
        public void NbtLongTest()
        {
            object dummy;
            NbtTag test = new NbtLong(9223372036854775807);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Equal((double)9223372036854775807, test.DoubleValue);
            Assert.Equal((float)9223372036854775807, test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Equal(9223372036854775807, test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Equal("9223372036854775807", test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtShortTest()
        {
            object dummy;
            NbtTag test = new NbtShort(32767);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Equal((double)32767, test.DoubleValue);
            Assert.Equal((float)32767, test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Equal(32767, test.IntValue);
            Assert.Equal(32767L, test.LongValue);
            Assert.Equal(32767, test.ShortValue);
            Assert.Equal("32767", test.StringValue);
            Assert.True(test.HasValue);
        }


        [Fact]
        public void NbtStringTest()
        {
            object dummy;
            NbtTag test = new NbtString("HELLO WORLD THIS IS A TEST STRING ÅÄÖ!");
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ByteValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.DoubleValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.FloatValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntArrayValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.IntValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.LongValue);
            Assert.Throws<InvalidCastException>(() => dummy = test.ShortValue);
            Assert.Equal("HELLO WORLD THIS IS A TEST STRING ÅÄÖ!", test.StringValue);
            Assert.True(test.HasValue);
        }
    }
}
