using Redstone.NBT.Tags;
using System;
using Xunit;

namespace Redstone.NBT.Tests
{
    public class MiscTests
    {
        [Fact]
        public void CopyConstructorTest()
        {
            NbtByte byteTag = new NbtByte("byteTag", 1);
            NbtByte byteTagClone = (NbtByte)byteTag.Clone();
            Assert.NotSame(byteTag, byteTagClone);
            Assert.Equal(byteTag.Name, byteTagClone.Name);
            Assert.Equal(byteTag.Value, byteTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtByte((NbtByte)null));

            NbtByteArray byteArrTag = new NbtByteArray("byteArrTag", new byte[] { 1, 2, 3, 4 });
            NbtByteArray byteArrTagClone = (NbtByteArray)byteArrTag.Clone();
            Assert.NotSame(byteArrTag, byteArrTagClone);
            Assert.Equal(byteArrTag.Name, byteArrTagClone.Name);
            Assert.NotSame(byteArrTag.Value, byteArrTagClone.Value);
            Assert.Equal(byteArrTag.Value, byteArrTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtByteArray((NbtByteArray)null));

            NbtCompound compTag = new NbtCompound("compTag", new NbtTag[] { new NbtByte("innerTag", 1) });
            NbtCompound compTagClone = (NbtCompound)compTag.Clone();
            Assert.NotSame(compTag, compTagClone);
            Assert.Equal(compTag.Name, compTagClone.Name);
            Assert.NotSame(compTag["innerTag"], compTagClone["innerTag"]);
            Assert.Equal(compTag["innerTag"].Name, compTagClone["innerTag"].Name);
            Assert.Equal(compTag["innerTag"].ByteValue, compTagClone["innerTag"].ByteValue);
            Assert.Throws<ArgumentNullException>(() => new NbtCompound((NbtCompound)null));

            NbtDouble doubleTag = new NbtDouble("doubleTag", 1);
            NbtDouble doubleTagClone = (NbtDouble)doubleTag.Clone();
            Assert.NotSame(doubleTag, doubleTagClone);
            Assert.Equal(doubleTag.Name, doubleTagClone.Name);
            Assert.Equal(doubleTag.Value, doubleTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtDouble((NbtDouble)null));

            NbtFloat floatTag = new NbtFloat("floatTag", 1);
            NbtFloat floatTagClone = (NbtFloat)floatTag.Clone();
            Assert.NotSame(floatTag, floatTagClone);
            Assert.Equal(floatTag.Name, floatTagClone.Name);
            Assert.Equal(floatTag.Value, floatTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtFloat((NbtFloat)null));

            NbtInt intTag = new NbtInt("intTag", 1);
            NbtInt intTagClone = (NbtInt)intTag.Clone();
            Assert.NotSame(intTag, intTagClone);
            Assert.Equal(intTag.Name, intTagClone.Name);
            Assert.Equal(intTag.Value, intTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtInt((NbtInt)null));

            NbtIntArray intArrTag = new NbtIntArray("intArrTag", new[] { 1, 2, 3, 4 });
            NbtIntArray intArrTagClone = (NbtIntArray)intArrTag.Clone();
            Assert.NotSame(intArrTag, intArrTagClone);
            Assert.Equal(intArrTag.Name, intArrTagClone.Name);
            Assert.NotSame(intArrTag.Value, intArrTagClone.Value);
            Assert.Equal(intArrTag.Value, intArrTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtIntArray((NbtIntArray)null));

            NbtList listTag = new NbtList("listTag", new NbtTag[] { new NbtByte(1) });
            NbtList listTagClone = (NbtList)listTag.Clone();
            Assert.NotSame(listTag, listTagClone);
            Assert.Equal(listTag.Name, listTagClone.Name);
            Assert.NotSame(listTag[0], listTagClone[0]);
            Assert.Equal(listTag[0].ByteValue, listTagClone[0].ByteValue);
            Assert.Throws<ArgumentNullException>(() => new NbtList((NbtList)null));

            NbtLong longTag = new NbtLong("longTag", 1);
            NbtLong longTagClone = (NbtLong)longTag.Clone();
            Assert.NotSame(longTag, longTagClone);
            Assert.Equal(longTag.Name, longTagClone.Name);
            Assert.Equal(longTag.Value, longTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtLong((NbtLong)null));

            NbtShort shortTag = new NbtShort("shortTag", 1);
            NbtShort shortTagClone = (NbtShort)shortTag.Clone();
            Assert.NotSame(shortTag, shortTagClone);
            Assert.Equal(shortTag.Name, shortTagClone.Name);
            Assert.Equal(shortTag.Value, shortTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtShort((NbtShort)null));

            NbtString stringTag = new NbtString("stringTag", "foo");
            NbtString stringTagClone = (NbtString)stringTag.Clone();
            Assert.NotSame(stringTag, stringTagClone);
            Assert.Equal(stringTag.Name, stringTagClone.Name);
            Assert.Equal(stringTag.Value, stringTagClone.Value);
            Assert.Throws<ArgumentNullException>(() => new NbtString((NbtString)null));
        }


        [Fact]
        public void ByteArrayIndexerTest()
        {
            // test getting/settings values of byte array tag via indexer
            var byteArray = new NbtByteArray("Test");
            Assert.Equal(new byte[0], byteArray.Value);
            byteArray.Value = new byte[] {
                1, 2, 3
            };
            Assert.Equal(1, byteArray[0]);
            Assert.Equal(2, byteArray[1]);
            Assert.Equal(3, byteArray[2]);
            byteArray[0] = 4;
            Assert.Equal(4, byteArray[0]);
        }


        [Fact]
        public void IntArrayIndexerTest()
        {
            // test getting/settings values of int array tag via indexer
            var intArray = new NbtIntArray("Test");
            Assert.Equal(new int[0], intArray.Value);
            intArray.Value = new[] {
                1, 2000, -3000000
            };
            Assert.Equal(1, intArray[0]);
            Assert.Equal(2000, intArray[1]);
            Assert.Equal(-3000000, intArray[2]);
            intArray[0] = 4;
            Assert.Equal(4, intArray[0]);
        }


        [Fact]
        public void DefaultValueTest()
        {
            // test default values of all value tags
            Assert.Equal(0, new NbtByte("test").Value);
            Assert.Equal(new byte[0], new NbtByteArray("test").Value);
            Assert.Equal(0d, new NbtDouble("test").Value);
            Assert.Equal(0f, new NbtFloat("test").Value);
            Assert.Equal(0, new NbtInt("test").Value);
            Assert.Equal(new int[0], new NbtIntArray("test").Value);
            Assert.Equal(0L, new NbtLong("test").Value);
            Assert.Equal(0, new NbtShort("test").Value);
            Assert.Equal("", new NbtString().Value);
        }


        [Fact]
        public void NullValueTest()
        {
            Assert.Throws<ArgumentNullException>(() => new NbtByteArray().Value = null);
            Assert.Throws<ArgumentNullException>(() => new NbtIntArray().Value = null);
            Assert.Throws<ArgumentNullException>(() => new NbtString().Value = null);
        }


        [Fact]
        public void PathTest()
        {
            // test NbtTag.Path property
            var testComp = new NbtCompound {
                new NbtCompound("Compound") {
                    new NbtCompound("InsideCompound")
                },
                new NbtList("List") {
                    new NbtCompound {
                        new NbtInt("InsideCompoundAndList")
                    }
                }
            };

            // parent-less tag with no name has empty string for a path
            Assert.Equal("", testComp.Path);
            Assert.Equal(".Compound", testComp["Compound"].Path);
            Assert.Equal(".Compound.InsideCompound", testComp["Compound"]["InsideCompound"].Path);
            Assert.Equal(".List", testComp["List"].Path);

            // tags inside lists have no name, but they do have an index
            Assert.Equal(".List[0]", testComp["List"][0].Path);
            Assert.Equal(".List[0].InsideCompoundAndList", testComp["List"][0]["InsideCompoundAndList"].Path);
        }


        [Fact]
        public void BadParamsTest()
        {
            Assert.Throws<ArgumentNullException>(() => new NbtByteArray((byte[])null));
            Assert.Throws<ArgumentNullException>(() => new NbtIntArray((int[])null));
            Assert.Throws<ArgumentNullException>(() => new NbtString((string)null));
        }
    }
}
