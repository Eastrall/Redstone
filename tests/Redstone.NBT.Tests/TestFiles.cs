using Redstone.NBT.Tags;
using System;
using System.IO;
using Xunit;

namespace Redstone.NBT.Tests
{
    public static class TestFiles
    {
        public const string DirName = "TestFiles";
        public static readonly string Small = Path.Combine(DirName, "test.nbt");
        public static readonly string SmallGZip = Path.Combine(DirName, "test.nbt.gz");
        public static readonly string SmallZLib = Path.Combine(DirName, "test.nbt.z");
        public static readonly string Big = Path.Combine(DirName, "bigtest.nbt");
        public static readonly string BigGZip = Path.Combine(DirName, "bigtest.nbt.gz");
        public static readonly string BigZLib = Path.Combine(DirName, "bigtest.nbt.z");


        // creates a compound containing lists of every kind of tag
        public static NbtCompound MakeListTest()
        {
            return new NbtCompound("Root") {
                new NbtList("ByteList") {
                    new NbtByte(100),
                    new NbtByte(20),
                    new NbtByte(3)
                },
                new NbtList("DoubleList") {
                    new NbtDouble(1d),
                    new NbtDouble(2000d),
                    new NbtDouble(-3000000d)
                },
                new NbtList("FloatList") {
                    new NbtFloat(1f),
                    new NbtFloat(2000f),
                    new NbtFloat(-3000000f)
                },
                new NbtList("IntList") {
                    new NbtInt(1),
                    new NbtInt(2000),
                    new NbtInt(-3000000)
                },
                new NbtList("LongList") {
                    new NbtLong(1L),
                    new NbtLong(2000L),
                    new NbtLong(-3000000L)
                },
                new NbtList("ShortList") {
                    new NbtShort(1),
                    new NbtShort(200),
                    new NbtShort(-30000)
                },
                new NbtList("StringList") {
                    new NbtString("one"),
                    new NbtString("two thousand"),
                    new NbtString("negative three million")
                },
                new NbtList("CompoundList") {
                    new NbtCompound(),
                    new NbtCompound(),
                    new NbtCompound()
                },
                new NbtList("ListList") {
                    new NbtList(NbtTagType.List),
                    new NbtList(NbtTagType.List),
                    new NbtList(NbtTagType.List)
                },
                new NbtList("ByteArrayList") {
                    new NbtByteArray(new byte[] {
                        1, 2, 3
                    }),
                    new NbtByteArray(new byte[] {
                        11, 12, 13
                    }),
                    new NbtByteArray(new byte[] {
                        21, 22, 23
                    })
                },
                new NbtList("IntArrayList") {
                    new NbtIntArray(new[] {
                        1, -2, 3
                    }),
                    new NbtIntArray(new[] {
                        1000, -2000, 3000
                    }),
                    new NbtIntArray(new[] {
                        1000000, -2000000, 3000000
                    })
                }
            };
        }


        // creates a file with lots of compounds and lists, used to test NbtReader compliance
        public static Stream MakeReaderTest()
        {
            var root = new NbtCompound("root") {
                new NbtInt("first"),
                new NbtInt("second"),
                new NbtCompound("third-comp") {
                    new NbtInt("inComp1"),
                    new NbtInt("inComp2"),
                    new NbtInt("inComp3")
                },
                new NbtList("fourth-list") {
                    new NbtList {
                        new NbtCompound {
                            new NbtCompound("inList1")
                        }
                    },
                    new NbtList {
                        new NbtCompound {
                            new NbtCompound("inList2")
                        }
                    },
                    new NbtList {
                        new NbtCompound {
                            new NbtCompound("inList3")
                        }
                    }
                },
                new NbtInt("fifth"),
                new NbtByteArray("hugeArray", new byte[1024*1024])
            };
            byte[] testData = new NbtFile(root).SaveToBuffer(NbtCompression.None);
            return new MemoryStream(testData);
        }


        // creates an NbtFile with contents identical to "test.nbt"
        public static NbtFile MakeSmallFile()
        {
            return new NbtFile(new NbtCompound("hello world") {
                new NbtString("name", "Bananrama")
            });
        }


        public static void AssertNbtSmallFile(NbtFile file)
        {
            // See TestFiles/test.nbt.txt to see the expected format
            Assert.IsType<NbtCompound>(file.RootTag);

            NbtCompound root = file.RootTag;
            Assert.Equal("hello world", root.Name);
            Assert.Single(root);

            Assert.IsType<NbtString>(root["name"]);

            var node = (NbtString)root["name"];
            Assert.Equal("name", node.Name);
            Assert.Equal("Bananrama", node.Value);
        }


        public static void AssertNbtBigFile(NbtFile file)
        {
            // See TestFiles/bigtest.nbt.txt to see the expected format
            Assert.IsType<NbtCompound>(file.RootTag);

            NbtCompound root = file.RootTag;
            Assert.Equal("Level", root.Name);
            Assert.Equal(12, root.Count);

            Assert.IsType<NbtLong>(root["longTest"]);
            NbtTag node = root["longTest"];
            Assert.Equal("longTest", node.Name);
            Assert.Equal(9223372036854775807, ((NbtLong)node).Value);

            Assert.IsType<NbtShort>(root["shortTest"]);
            node = root["shortTest"];
            Assert.Equal("shortTest", node.Name);
            Assert.Equal(32767, ((NbtShort)node).Value);

            Assert.IsType<NbtString>(root["stringTest"]);
            node = root["stringTest"];
            Assert.Equal("stringTest", node.Name);
            Assert.Equal("HELLO WORLD THIS IS A TEST STRING ÅÄÖ!", ((NbtString)node).Value);

            Assert.IsType<NbtFloat>(root["floatTest"]);
            node = root["floatTest"];
            Assert.Equal("floatTest", node.Name);
            Assert.Equal(0.49823147f, ((NbtFloat)node).Value);

            Assert.IsType<NbtInt>(root["intTest"]);
            node = root["intTest"];
            Assert.Equal("intTest", node.Name);
            Assert.Equal(2147483647, ((NbtInt)node).Value);

            Assert.IsType<NbtCompound>(root["nested compound test"]);
            node = root["nested compound test"];
            Assert.Equal("nested compound test", node.Name);
            Assert.Equal(2, ((NbtCompound)node).Count);

            // First nested test
            Assert.IsType<NbtCompound>(node["ham"]);
            var subNode = (NbtCompound)node["ham"];
            Assert.Equal("ham", subNode.Name);
            Assert.Equal(2, subNode.Count);

            // Checking sub node values
            Assert.IsType<NbtString>(subNode["name"]);
            Assert.Equal("name", subNode["name"].Name);
            Assert.Equal("Hampus", ((NbtString)subNode["name"]).Value);

            Assert.IsType<NbtFloat>(subNode["value"]);
            Assert.Equal("value", subNode["value"].Name);
            Assert.Equal(0.75, ((NbtFloat)subNode["value"]).Value);
            // End sub node

            // Second nested test
            Assert.IsType<NbtCompound>(node["egg"]);
            subNode = (NbtCompound)node["egg"];
            Assert.Equal("egg", subNode.Name);
            Assert.Equal(2, subNode.Count);

            // Checking sub node values
            Assert.IsType<NbtString>(subNode["name"]);
            Assert.Equal("name", subNode["name"].Name);
            Assert.Equal("Eggbert", ((NbtString)subNode["name"]).Value);

            Assert.IsType<NbtFloat>(subNode["value"]);
            Assert.Equal("value", subNode["value"].Name);
            Assert.Equal(0.5, ((NbtFloat)subNode["value"]).Value);
            // End sub node

            Assert.IsType<NbtList>(root["listTest (long)"]);
            node = root["listTest (long)"];
            Assert.Equal("listTest (long)", node.Name);
            Assert.Equal(5, ((NbtList)node).Count);

            // The values should be: 11, 12, 13, 14, 15
            for (int nodeIndex = 0; nodeIndex < ((NbtList)node).Count; nodeIndex++)
            {
                Assert.IsType<NbtLong>(node[nodeIndex]);
                Assert.Null(node[nodeIndex].Name);
                Assert.Equal(nodeIndex + 11, ((NbtLong)node[nodeIndex]).Value);
            }

            Assert.IsType<NbtList>(root["listTest (compound)"]);
            node = root["listTest (compound)"];
            Assert.Equal("listTest (compound)", node.Name);
            Assert.Equal(2, ((NbtList)node).Count);

            // First Sub Node
            Assert.IsType<NbtCompound>(node[0]);
            subNode = (NbtCompound)node[0];

            // First node in sub node
            Assert.IsType<NbtString>(subNode["name"]);
            Assert.Equal("name", subNode["name"].Name);
            Assert.Equal("Compound tag #0", ((NbtString)subNode["name"]).Value);

            // Second node in sub node
            Assert.IsType<NbtLong>(subNode["created-on"]);
            Assert.Equal("created-on", subNode["created-on"].Name);
            Assert.Equal(1264099775885, ((NbtLong)subNode["created-on"]).Value);

            // Second Sub Node
            Assert.IsType<NbtCompound>(node[1]);
            subNode = (NbtCompound)node[1];

            // First node in sub node
            Assert.IsType<NbtString>(subNode["name"]);
            Assert.Equal("name", subNode["name"].Name);
            Assert.Equal("Compound tag #1", ((NbtString)subNode["name"]).Value);

            // Second node in sub node
            Assert.IsType<NbtLong>(subNode["created-on"]);
            Assert.Equal("created-on", subNode["created-on"].Name);
            Assert.Equal(1264099775885, ((NbtLong)subNode["created-on"]).Value);

            Assert.IsType<NbtByte>(root["byteTest"]);
            node = root["byteTest"];
            Assert.Equal("byteTest", node.Name);
            Assert.Equal(127, ((NbtByte)node).Value);

            const string byteArrayName =
                "byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))";
            Assert.IsType<NbtByteArray>(root[byteArrayName]);
            node = root[byteArrayName];
            Assert.Equal(byteArrayName, node.Name);
            Assert.Equal(1000, ((NbtByteArray)node).Value.Length);

            // Values are: the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...)
            for (int n = 0; n < 1000; n++)
            {
                Assert.Equal((n * n * 255 + n * 7) % 100, ((NbtByteArray)node)[n]);
            }

            Assert.IsType<NbtDouble>(root["doubleTest"]);
            node = root["doubleTest"];
            Assert.Equal("doubleTest", node.Name);
            Assert.Equal(0.4931287132182315, ((NbtDouble)node).Value);

            Assert.IsType<NbtIntArray>(root["intArrayTest"]);
            var intArrayTag = root.Get<NbtIntArray>("intArrayTest");
            Assert.NotNull(intArrayTag);
            var rand = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(rand.Next(), intArrayTag.Value[i]);
            }
        }


        #region Value test

        // creates an NbtCompound with one of tag of each value-type
        public static NbtCompound MakeValueTest()
        {
            return new NbtCompound("root") {
                new NbtByte("byte", 1),
                new NbtShort("short", 2),
                new NbtInt("int", 3),
                new NbtLong("long", 4L),
                new NbtFloat("float", 5f),
                new NbtDouble("double", 6d),
                new NbtByteArray("byteArray", new byte[] { 10, 11, 12 }),
                new NbtIntArray("intArray", new[] { 20, 21, 22 }),
                new NbtString("string", "123")
            };
        }


        public static void AssertValueTest(NbtFile file)
        {
            Assert.IsType<NbtCompound>(file.RootTag);

            NbtCompound root = file.RootTag;
            Assert.Equal("root", root.Name);
            Assert.Equal(9, root.Count);

            Assert.IsType<NbtByte>(root["byte"]);
            NbtTag node = root["byte"];
            Assert.Equal("byte", node.Name);
            Assert.Equal(1, node.ByteValue);

            Assert.IsType<NbtShort>(root["short"]);
            node = root["short"];
            Assert.Equal("short", node.Name);
            Assert.Equal(2, node.ShortValue);

            Assert.IsType<NbtInt>(root["int"]);
            node = root["int"];
            Assert.Equal("int", node.Name);
            Assert.Equal(3, node.IntValue);

            Assert.IsType<NbtLong>(root["long"]);
            node = root["long"];
            Assert.Equal("long", node.Name);
            Assert.Equal(4L, node.LongValue);

            Assert.IsType<NbtFloat>(root["float"]);
            node = root["float"];
            Assert.Equal("float", node.Name);
            Assert.Equal(5f, node.FloatValue);

            Assert.IsType<NbtDouble>(root["double"]);
            node = root["double"];
            Assert.Equal("double", node.Name);
            Assert.Equal(6d, node.DoubleValue);

            Assert.IsType<NbtByteArray>(root["byteArray"]);
            node = root["byteArray"];
            Assert.Equal("byteArray", node.Name);
            Assert.Equal(new byte[] { 10, 11, 12 }, node.ByteArrayValue);

            Assert.IsType<NbtIntArray>(root["intArray"]);
            node = root["intArray"];
            Assert.Equal("intArray", node.Name);
            Assert.Equal(new[] { 20, 21, 22 }, node.IntArrayValue);

            Assert.IsType<NbtString>(root["string"]);
            node = root["string"];
            Assert.Equal("string", node.Name);
            Assert.Equal("123", node.StringValue);
        }

        #endregion
    }
}
