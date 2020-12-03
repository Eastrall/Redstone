using Redstone.NBT.Exceptions;
using Redstone.NBT.Tags;
using System;
using System.IO;
using Xunit;

namespace Redstone.NBT.Tests
{
    public sealed class NbtReaderTests
    {
        [Fact]
        public void PrintBigFileUncompressed()
        {
            using FileStream fs = File.OpenRead("TestFiles/bigtest.nbt");
            
            var reader = new NbtReader(fs);
            Assert.Equal(fs, reader.BaseStream);
            while (reader.ReadToFollowing())
            {
                Console.Write("@" + reader.TagStartOffset + " ");
                Console.WriteLine(reader.ToString());
            }
            Assert.Equal("Level", reader.RootName);
        }


        [Fact]
        public void PrintBigFileUncompressedNoSkip()
        {
            using FileStream fs = File.OpenRead("TestFiles/bigtest.nbt");
            
            var reader = new NbtReader(fs)
            {
                SkipEndTags = false
            };
            Assert.Equal(fs, reader.BaseStream);
            while (reader.ReadToFollowing())
            {
                Console.Write("@" + reader.TagStartOffset + " ");
                Console.WriteLine(reader.ToString());
            }
            Assert.Equal("Level", reader.RootName);
        }


        [Fact]
        public void CacheTagValuesTest()
        {
            byte[] testData = new NbtFile(TestFiles.MakeValueTest()).SaveToBuffer(NbtCompression.None);
            var reader = new NbtReader(new MemoryStream(testData));
            Assert.False(reader.CacheTagValues);
            reader.CacheTagValues = true;
            Assert.True(reader.ReadToFollowing()); // root

            Assert.True(reader.ReadToFollowing()); // byte
            Assert.Equal((byte)1, reader.ReadValue());
            Assert.Equal((byte)1, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // short
            Assert.Equal((short)2, reader.ReadValue());
            Assert.Equal((short)2, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // int
            Assert.Equal(3, reader.ReadValue());
            Assert.Equal(3, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // long
            Assert.Equal(4L, reader.ReadValue());
            Assert.Equal(4L, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // float
            Assert.Equal(5f, reader.ReadValue());
            Assert.Equal(5f, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // double
            Assert.Equal(6d, reader.ReadValue());
            Assert.Equal(6d, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // byteArray
            Assert.Equal(new byte[] { 10, 11, 12 }, (byte[])reader.ReadValue());
            Assert.Equal(new byte[] { 10, 11, 12 }, (byte[])reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // intArray
            Assert.Equal(new[] { 20, 21, 22 }, (int[])reader.ReadValue());
            Assert.Equal(new[] { 20, 21, 22 }, (int[])reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // string
            Assert.Equal("123", reader.ReadValue());
            Assert.Equal("123", reader.ReadValue());
        }


        [Fact]
        public void NestedListTest()
        {
            var root = new NbtCompound("root") {
                new NbtList("OuterList") {
                    new NbtList {
                        new NbtByte()
                    },
                    new NbtList {
                        new NbtShort()
                    },
                    new NbtList {
                        new NbtInt()
                    }
                }
            };
            byte[] testData = new NbtFile(root).SaveToBuffer(NbtCompression.None);
            
            using var ms = new MemoryStream(testData);
            var reader = new NbtReader(ms);
            while (reader.ReadToFollowing())
            {
                Console.WriteLine(reader.ToString(true));
            }
        }


        [Fact]
        public void PropertiesTest()
        {
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            Assert.Equal(0, reader.Depth);
            Assert.Equal(0, reader.TagsRead);

            Assert.True(reader.ReadToFollowing());
            Assert.Equal("root", reader.TagName);
            Assert.Equal(NbtTagType.Compound, reader.TagType);
            Assert.Equal(NbtTagType.Unknown, reader.ListType);
            Assert.False(reader.HasValue);
            Assert.True(reader.IsCompound);
            Assert.False(reader.IsList);
            Assert.False(reader.IsListElement);
            Assert.False(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(1, reader.Depth);
            Assert.Null(reader.ParentName);
            Assert.Equal(NbtTagType.Unknown, reader.ParentTagType);
            Assert.Equal(0, reader.ParentTagLength);
            Assert.Equal(0, reader.TagLength);
            Assert.Equal(1, reader.TagsRead);

            Assert.True(reader.ReadToFollowing());
            Assert.Equal("first", reader.TagName);
            Assert.Equal(NbtTagType.Int, reader.TagType);
            Assert.Equal(NbtTagType.Unknown, reader.ListType);
            Assert.True(reader.HasValue);
            Assert.False(reader.IsCompound);
            Assert.False(reader.IsList);
            Assert.False(reader.IsListElement);
            Assert.False(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(2, reader.Depth);
            Assert.Equal("root", reader.ParentName);
            Assert.Equal(NbtTagType.Compound, reader.ParentTagType);
            Assert.Equal(0, reader.ParentTagLength);
            Assert.Equal(0, reader.TagLength);
            Assert.Equal(2, reader.TagsRead);

            Assert.True(reader.ReadToFollowing("fourth-list"));
            Assert.Equal("fourth-list", reader.TagName);
            Assert.Equal(NbtTagType.List, reader.TagType);
            Assert.Equal(NbtTagType.List, reader.ListType);
            Assert.False(reader.HasValue);
            Assert.False(reader.IsCompound);
            Assert.True(reader.IsList);
            Assert.False(reader.IsListElement);
            Assert.True(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(2, reader.Depth);
            Assert.Equal("root", reader.ParentName);
            Assert.Equal(NbtTagType.Compound, reader.ParentTagType);
            Assert.Equal(0, reader.ParentTagLength);
            Assert.Equal(3, reader.TagLength);
            Assert.Equal(8, reader.TagsRead);

            Assert.True(reader.ReadToFollowing()); // first list element, itself a list
            Assert.Null(reader.TagName);
            Assert.Equal(NbtTagType.List, reader.TagType);
            Assert.Equal(NbtTagType.Compound, reader.ListType);
            Assert.False(reader.HasValue);
            Assert.False(reader.IsCompound);
            Assert.True(reader.IsList);
            Assert.True(reader.IsListElement);
            Assert.True(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(3, reader.Depth);
            Assert.Equal("fourth-list", reader.ParentName);
            Assert.Equal(NbtTagType.List, reader.ParentTagType);
            Assert.Equal(3, reader.ParentTagLength);
            Assert.Equal(1, reader.TagLength);
            Assert.Equal(9, reader.TagsRead);

            Assert.True(reader.ReadToFollowing()); // first nested list element, compound
            Assert.Null(reader.TagName);
            Assert.Equal(NbtTagType.Compound, reader.TagType);
            Assert.Equal(NbtTagType.Unknown, reader.ListType);
            Assert.False(reader.HasValue);
            Assert.True(reader.IsCompound);
            Assert.False(reader.IsList);
            Assert.True(reader.IsListElement);
            Assert.False(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(4, reader.Depth);
            Assert.Null(reader.ParentName);
            Assert.Equal(NbtTagType.List, reader.ParentTagType);
            Assert.Equal(1, reader.ParentTagLength);
            Assert.Equal(0, reader.TagLength);
            Assert.Equal(10, reader.TagsRead);

            Assert.True(reader.ReadToFollowing("fifth"));
            Assert.Equal("fifth", reader.TagName);
            Assert.Equal(NbtTagType.Int, reader.TagType);
            Assert.Equal(NbtTagType.Unknown, reader.ListType);
            Assert.True(reader.HasValue);
            Assert.False(reader.IsCompound);
            Assert.False(reader.IsList);
            Assert.False(reader.IsListElement);
            Assert.False(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(2, reader.Depth);
            Assert.Equal("root", reader.ParentName);
            Assert.Equal(NbtTagType.Compound, reader.ParentTagType);
            Assert.Equal(0, reader.ParentTagLength);
            Assert.Equal(0, reader.TagLength);
            Assert.Equal(18, reader.TagsRead);

            Assert.True(reader.ReadToFollowing());
            Assert.Equal("hugeArray", reader.TagName);
            Assert.Equal(NbtTagType.ByteArray, reader.TagType);
            Assert.Equal(NbtTagType.Unknown, reader.ListType);
            Assert.True(reader.HasValue);
            Assert.False(reader.IsCompound);
            Assert.False(reader.IsList);
            Assert.False(reader.IsListElement);
            Assert.True(reader.HasLength);
            Assert.Equal(0, reader.ListIndex);
            Assert.Equal(2, reader.Depth);
            Assert.Equal("root", reader.ParentName);
            Assert.Equal(NbtTagType.Compound, reader.ParentTagType);
            Assert.Equal(0, reader.ParentTagLength);
            Assert.Equal(1024 * 1024, reader.TagLength);
            Assert.Equal(19, reader.TagsRead);
        }


        [Fact]
        public void ReadToSiblingTest()
        {
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            Assert.True(reader.ReadToFollowing());
            Assert.Equal("root", reader.TagName);
            Assert.True(reader.ReadToFollowing());
            Assert.Equal("first", reader.TagName);
            Assert.True(reader.ReadToNextSibling("third-comp"));
            Assert.Equal("third-comp", reader.TagName);
            Assert.True(reader.ReadToNextSibling());
            Assert.Equal("fourth-list", reader.TagName);
            Assert.True(reader.ReadToNextSibling());
            Assert.Equal("fifth", reader.TagName);
            Assert.True(reader.ReadToNextSibling());
            Assert.Equal("hugeArray", reader.TagName);
            Assert.False(reader.ReadToNextSibling());
            // Test twice, since we hit different paths through the code
            Assert.False(reader.ReadToNextSibling());
        }


        [Fact]
        public void ReadToSiblingTest2()
        {
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            Assert.True(reader.ReadToFollowing("inComp1"));
            // Expect all siblings to be read while we search for a non-existent one
            Assert.False(reader.ReadToNextSibling("no such tag"));
            // Expect to pop out of "third-comp" by now
            Assert.Equal("fourth-list", reader.TagName);
        }


        [Fact]
        public void ReadToFollowingNotFound()
        {
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            Assert.True(reader.ReadToFollowing()); // at "root"
            Assert.False(reader.ReadToFollowing("no such tag"));
            Assert.False(reader.ReadToFollowing("not this one either"));
            Assert.True(reader.IsAtStreamEnd);
        }


        [Fact]
        public void ReadToDescendantTest()
        {
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            Assert.True(reader.ReadToDescendant("third-comp"));
            Assert.Equal("third-comp", reader.TagName);
            Assert.True(reader.ReadToDescendant("inComp2"));
            Assert.Equal("inComp2", reader.TagName);
            Assert.False(reader.ReadToDescendant("derp"));
            Assert.Equal("inComp3", reader.TagName);
            reader.ReadToFollowing(); // at fourth-list
            Assert.True(reader.ReadToDescendant("inList2"));
            Assert.Equal("inList2", reader.TagName);

            // Ensure ReadToDescendant returns false when at end-of-stream
            while (reader.ReadToFollowing()) { }
            Assert.False(reader.ReadToDescendant("*"));
        }


        [Fact]
        public void SkipTest()
        {
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            reader.ReadToFollowing(); // at root
            reader.ReadToFollowing(); // at first
            reader.ReadToFollowing(); // at second
            reader.ReadToFollowing(); // at third-comp
            reader.ReadToFollowing(); // at inComp1
            Assert.Equal("inComp1", reader.TagName);
            Assert.Equal(2, reader.Skip());
            Assert.Equal("fourth-list", reader.TagName);
            Assert.Equal(11, reader.Skip());
            Assert.False(reader.ReadToFollowing());
            Assert.Equal(0, reader.Skip());
        }


        [Fact]
        public void ReadAsTagTest1()
        {
            // read various lists/compounds as tags
            var reader = new NbtReader(TestFiles.MakeReaderTest());
            reader.ReadToFollowing(); // skip root
            while (!reader.IsAtStreamEnd)
            {
                reader.ReadAsTag();
            }
            Assert.Throws<EndOfStreamException>(() => reader.ReadAsTag());
        }


        [Fact]
        public void ReadAsTagTest2()
        {
            // read the whole thing as one tag
            byte[] testData = new NbtFile(TestFiles.MakeValueTest()).SaveToBuffer(NbtCompression.None);
            {
                var reader = new NbtReader(new MemoryStream(testData));
                var root = (NbtCompound)reader.ReadAsTag();
                TestFiles.AssertValueTest(new NbtFile(root));
            }
            {
                // Try the same thing but with end tag skipping disabled
                var reader = new NbtReader(new MemoryStream(testData))
                {
                    SkipEndTags = false
                };
                var root = (NbtCompound)reader.ReadAsTag();
                TestFiles.AssertValueTest(new NbtFile(root));
            }
        }


        [Fact]
        public void ReadAsTagTest3()
        {
            // read values as tags
            byte[] testData = new NbtFile(TestFiles.MakeValueTest()).SaveToBuffer(NbtCompression.None);
            var reader = new NbtReader(new MemoryStream(testData));
            var root = new NbtCompound("root");

            // skip root
            reader.ReadToFollowing();
            reader.ReadToFollowing();

            while (!reader.IsAtStreamEnd)
            {
                root.Add(reader.ReadAsTag());
            }

            TestFiles.AssertValueTest(new NbtFile(root));
        }


        [Fact]
        public void ReadAsTagTest4()
        {
            // read a bunch of lists as tags
            byte[] testData = new NbtFile(TestFiles.MakeListTest()).SaveToBuffer(NbtCompression.None);

            // first, read everything all-at-once
            {
                var reader = new NbtReader(new MemoryStream(testData));
                while (!reader.IsAtStreamEnd)
                {
                    Console.WriteLine(reader.ReadAsTag());
                }
            }

            // next, read each list individually
            {
                var reader = new NbtReader(new MemoryStream(testData));
                reader.ReadToFollowing(); // read to root
                reader.ReadToFollowing(); // read to first list tag
                while (!reader.IsAtStreamEnd)
                {
                    Console.WriteLine(reader.ReadAsTag());
                }
            }
        }


        [Fact]
        public void ReadListAsArray()
        {
            NbtCompound intList = TestFiles.MakeListTest();

            var ms = new MemoryStream();
            new NbtFile(intList).SaveToStream(ms, NbtCompression.None);
            ms.Seek(0, SeekOrigin.Begin);
            var reader = new NbtReader(ms);

            // attempt to read value before we're in a list
            Assert.Throws<InvalidOperationException>(() => reader.ReadListAsArray<int>());

            // test byte values
            reader.ReadToFollowing("ByteList");
            byte[] bytes = reader.ReadListAsArray<byte>();
            Assert.Equal(new byte[] { 100, 20, 3 }, bytes);

            // test double values
            reader.ReadToFollowing("DoubleList");
            double[] doubles = reader.ReadListAsArray<double>();
            Assert.Equal(new[] { 1d, 2000d, -3000000d }, doubles);

            // test float values
            reader.ReadToFollowing("FloatList");
            float[] floats = reader.ReadListAsArray<float>();
            Assert.Equal(new[] { 1f, 2000f, -3000000f }, floats);

            // test int values
            reader.ReadToFollowing("IntList");
            int[] ints = reader.ReadListAsArray<int>();
            Assert.Equal(new[] { 1, 2000, -3000000 }, ints);

            // test long values
            reader.ReadToFollowing("LongList");
            long[] longs = reader.ReadListAsArray<long>();
            Assert.Equal(new[] { 1L, 2000L, -3000000L }, longs);

            // test short values
            reader.ReadToFollowing("ShortList");
            short[] shorts = reader.ReadListAsArray<short>();
            Assert.Equal(new short[] { 1, 200, -30000 }, shorts);

            // test short values
            reader.ReadToFollowing("StringList");
            string[] strings = reader.ReadListAsArray<string>();
            Assert.Equal(new[] { "one", "two thousand", "negative three million" }, strings);

            // try reading list of compounds (should fail)
            reader.ReadToFollowing("CompoundList");
            Assert.Throws<InvalidOperationException>(() => reader.ReadListAsArray<NbtCompound>());

            // skip to the end of the stream
            while (reader.ReadToFollowing()) { }
            Assert.Throws<EndOfStreamException>(() => reader.ReadListAsArray<int>());
        }


        [Fact]
        public void ReadListAsArrayRecast()
        {
            NbtCompound intList = TestFiles.MakeListTest();

            var ms = new MemoryStream();
            new NbtFile(intList).SaveToStream(ms, NbtCompression.None);
            ms.Seek(0, SeekOrigin.Begin);
            var reader = new NbtReader(ms);

            // test bytes as shorts
            reader.ReadToFollowing("ByteList");
            short[] bytes = reader.ReadListAsArray<short>();
            Assert.Equal(bytes,
                                      new short[] {
                                          100, 20, 3
                                      });
        }


        [Fact]
        public void ReadValueTest()
        {
            byte[] testData = new NbtFile(TestFiles.MakeValueTest()).SaveToBuffer(NbtCompression.None);
            var reader = new NbtReader(new MemoryStream(testData));

            Assert.True(reader.ReadToFollowing()); // root

            Assert.True(reader.ReadToFollowing()); // byte
            Assert.Equal((byte)1, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // short
            Assert.Equal((short)2, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // int
            Assert.Equal((int)3, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // long
            Assert.Equal(4L, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // float
            Assert.Equal(5f, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // double
            Assert.Equal(6d, reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // byteArray
            Assert.Equal(new byte[] { 10, 11, 12 }, (byte[])reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // intArray
            Assert.Equal(new[] { 20, 21, 22 }, (int[])reader.ReadValue());
            Assert.True(reader.ReadToFollowing()); // string
            Assert.Equal("123", reader.ReadValue());

            // Skip to the very end and make sure that we can't read any more values
            reader.ReadToFollowing();
            Assert.Throws<EndOfStreamException>(() => reader.ReadValue());
        }


        [Fact]
        public void ReadValueAsTest()
        {
            byte[] testData = new NbtFile(TestFiles.MakeValueTest()).SaveToBuffer(NbtCompression.None);
            var reader = new NbtReader(new MemoryStream(testData));

            Assert.True(reader.ReadToFollowing()); // root

            Assert.True(reader.ReadToFollowing()); // byte
            Assert.Equal(1, reader.ReadValueAs<byte>());
            Assert.True(reader.ReadToFollowing()); // short
            Assert.Equal(2, reader.ReadValueAs<short>());
            Assert.True(reader.ReadToFollowing()); // int
            Assert.Equal(3, reader.ReadValueAs<int>());
            Assert.True(reader.ReadToFollowing()); // long
            Assert.Equal(4L, reader.ReadValueAs<long>());
            Assert.True(reader.ReadToFollowing()); // float
            Assert.Equal(5f, reader.ReadValueAs<float>());
            Assert.True(reader.ReadToFollowing()); // double
            Assert.Equal(6d, reader.ReadValueAs<double>());
            Assert.True(reader.ReadToFollowing()); // byteArray
            Assert.Equal(new byte[] { 10, 11, 12 }, reader.ReadValueAs<byte[]>());
            Assert.True(reader.ReadToFollowing()); // intArray
            Assert.Equal(new[] { 20, 21, 22 }, reader.ReadValueAs<int[]>());
            Assert.True(reader.ReadToFollowing()); // string
            Assert.Equal("123", reader.ReadValueAs<string>());
        }


        [Fact]
        public void ErrorTest()
        {
            var root = new NbtCompound("root");
            byte[] testData = new NbtFile(root).SaveToBuffer(NbtCompression.None);

            // creating NbtReader without a stream, or with a non-readable stream
            Assert.Throws<ArgumentNullException>(() => new NbtReader(null));
            Assert.Throws<ArgumentException>(() => new NbtReader(new NonReadableStream()));

            // corrupt the data
            testData[0] = 123;
            var reader = new NbtReader(new MemoryStream(testData));

            // attempt to use ReadValue when not at value
            Assert.Throws<InvalidOperationException>(() => reader.ReadValue());
            reader.CacheTagValues = true;
            Assert.Throws<InvalidOperationException>(() => reader.ReadValue());

            // attempt to read a corrupt stream
            Assert.Throws<NbtFormatException>(() => reader.ReadToFollowing());

            // make sure we've properly entered the error state
            Assert.True(reader.IsInErrorState);
            Assert.False(reader.HasName);
            Assert.Throws<InvalidReaderStateException>(() => reader.ReadToFollowing());
            Assert.Throws<InvalidReaderStateException>(() => reader.ReadListAsArray<int>());
            Assert.Throws<InvalidReaderStateException>(() => reader.ReadToNextSibling());
            Assert.Throws<InvalidReaderStateException>(() => reader.ReadToDescendant("derp"));
            Assert.Throws<InvalidReaderStateException>(() => reader.ReadAsTag());
            Assert.Throws<InvalidReaderStateException>(() => reader.Skip());
        }


        [Fact]
        public void NonSeekableStreamSkip1()
        {
            byte[] fileBytes = File.ReadAllBytes("TestFiles/bigtest.nbt");
            using var ms = new MemoryStream(fileBytes);
            using var nss = new NonSeekableStream(ms);
            
            var reader = new NbtReader(nss);
            reader.ReadToFollowing();
            reader.Skip();
        }


        [Fact]
        public void NonSeekableStreamSkip2()
        {
            using var ms = TestFiles.MakeReaderTest();
            using var nss = new NonSeekableStream(ms);
            
            var reader = new NbtReader(nss);
            reader.ReadToFollowing();
            reader.Skip();
        }


        [Fact]
        public void CorruptFileRead()
        {
            byte[] emptyFile = new byte[0];
            Assert.Throws<EndOfStreamException>(() => TryReadBadFile(emptyFile));
            Assert.Throws<EndOfStreamException>(
                () => new NbtFile().LoadFromBuffer(emptyFile, 0, emptyFile.Length, NbtCompression.None));
            Assert.Throws<EndOfStreamException>(
                () => NbtFile.ReadRootTagName(new MemoryStream(emptyFile), NbtCompression.AutoDetect, true, 0));
            Assert.Throws<EndOfStreamException>(
                () => NbtFile.ReadRootTagName(new MemoryStream(emptyFile), NbtCompression.None, true, 0));

            byte[] badHeader = {
                0x02, // TAG_Short ID (instead of TAG_Compound ID)
                0x00, 0x01, 0x66, // Root name: 'f'
                0x00 // end tag
            };
            Assert.Throws<NbtFormatException>(() => TryReadBadFile(badHeader));
            Assert.Throws<NbtFormatException>(
                () => new NbtFile().LoadFromBuffer(badHeader, 0, badHeader.Length, NbtCompression.None));
            Assert.Throws<NbtFormatException>(
                () => NbtFile.ReadRootTagName(new MemoryStream(badHeader), NbtCompression.None, true, 0));

            byte[] badStringLength = {
                0x0A, // Compound tag
                0xFF, 0xFF, 0x66, // Root name 'f' (with string length given as "-1")
                0x00 // end tag
            };
            Assert.Throws<NbtFormatException>(() => TryReadBadFile(badStringLength));
            Assert.Throws<NbtFormatException>(
                () => new NbtFile().LoadFromBuffer(badStringLength, 0, badStringLength.Length, NbtCompression.None));
            Assert.Throws<NbtFormatException>(
                () => NbtFile.ReadRootTagName(new MemoryStream(badStringLength), NbtCompression.None, true, 0));

            byte[] abruptStringEnd = {
                0x0A, // Compound tag
                0x00, 0xFF, 0x66, // Root name 'f' (string length given as 5)
                0x00 // premature end tag
            };
            Assert.Throws<EndOfStreamException>(() => TryReadBadFile(abruptStringEnd));
            Assert.Throws<EndOfStreamException>(
                () => new NbtFile().LoadFromBuffer(abruptStringEnd, 0, abruptStringEnd.Length, NbtCompression.None));
            Assert.Throws<EndOfStreamException>(
                () => NbtFile.ReadRootTagName(new MemoryStream(abruptStringEnd), NbtCompression.None, true, 0));

            byte[] badSecondTag = {
                0x0A, // Compound tag
                0x00, 0x01, 0x66, // Root name: 'f'
                0xFF, 0x01, 0x4E, 0x7F, 0xFF, // Short tag named 'N' with invalid tag ID (0xFF instead of 0x02)
                0x00 // end tag
            };
            Assert.Throws<NbtFormatException>(() => TryReadBadFile(badSecondTag));
            Assert.Throws<NbtFormatException>(
                () => new NbtFile().LoadFromBuffer(badSecondTag, 0, badSecondTag.Length, NbtCompression.None));

            byte[] badListType = {
                0x0A, // Compound tag
                0x00, 0x01, 0x66, // Root name: 'f'
                0x09, // List tag
                0x00, 0x01, 0x66, // List tag name: 'g'
                0xFF // invalid list tag type (-1)
            };
            Assert.Throws<NbtFormatException>(() => TryReadBadFile(badListType));
            Assert.Throws<NbtFormatException>(
                () => new NbtFile().LoadFromBuffer(badListType, 0, badListType.Length, NbtCompression.None));

            byte[] badListSize = {
                0x0A, // Compound tag
                0x00, 0x01, 0x66, // Root name: 'f'
                0x09, // List tag
                0x00, 0x01, 0x66, // List tag name: 'g'
                0x01, // List type: Byte
                0xFF, 0xFF, 0xFF, 0xFF, // List size: -1
            };
            Assert.Throws<NbtFormatException>(() => TryReadBadFile(badListSize));
            Assert.Throws<NbtFormatException>(
                () => new NbtFile().LoadFromBuffer(badListSize, 0, badListSize.Length, NbtCompression.None));
        }


        [Fact]
        public void PartialReadTest()
        {
            // read the whole thing as one tag, one byte at a time
            TestFiles.AssertValueTest(PartialReadTestInternal(new NbtFile(TestFiles.MakeValueTest())));
            TestFiles.AssertNbtSmallFile(PartialReadTestInternal(TestFiles.MakeSmallFile()));
            TestFiles.AssertNbtBigFile(PartialReadTestInternal(new NbtFile(TestFiles.Big)));
        }


        [Fact]
        public void PartialBatchReadTest()
        {
            // read the whole thing as one tag, in batches of 4 bytes
            // Verifies fix for https://github.com/fragmer/fNbt/issues/26
            TestFiles.AssertValueTest(PartialReadTestInternal(new NbtFile(TestFiles.MakeValueTest()), 4));
            TestFiles.AssertNbtSmallFile(PartialReadTestInternal(TestFiles.MakeSmallFile(), 4));
            TestFiles.AssertNbtBigFile(PartialReadTestInternal(new NbtFile(TestFiles.Big), 4));
        }


        [Fact]
        public void EndTagTest()
        {
            using MemoryStream ms = new MemoryStream();
            
            var root = new NbtCompound("root") {
                    new NbtInt("test", 0)
                };
            new NbtFile(root).SaveToStream(ms, NbtCompression.None);
            ms.Position = 0;

            NbtReader reader = new NbtReader(ms) { SkipEndTags = false };
            reader.ReadToDescendant("test");
            Assert.Equal(NbtTagType.Int, reader.TagType);
            Assert.True(reader.ReadToNextSibling());

            // should be at root's End tag now
            Assert.Equal(NbtTagType.End, reader.TagType);
            Assert.False(reader.IsInErrorState);
            Assert.False(reader.IsAtStreamEnd);
            Assert.False(reader.IsCompound);
            Assert.False(reader.IsList);
            Assert.False(reader.IsListElement);
            Assert.False(reader.HasValue);
            Assert.False(reader.HasName);
            Assert.False(reader.HasLength);
            Assert.Throws<InvalidOperationException>(() => reader.ReadAsTag()); // Cannot create NbtTag from TAG_END

            // We done now
            Assert.False(reader.ReadToFollowing());
            Assert.True(reader.IsAtStreamEnd);
        }


        static NbtFile PartialReadTestInternal(NbtFile comp, int increment = 1)
        {
            byte[] testData = comp.SaveToBuffer(NbtCompression.None);
            var reader = new NbtReader(new PartialReadStream(new MemoryStream(testData), increment));
            var root = (NbtCompound)reader.ReadAsTag();
            return new NbtFile(root);
        }


        void TryReadBadFile(byte[] data)
        {
            using MemoryStream ms = new MemoryStream(data);
           
            NbtReader reader = new NbtReader(ms);
            try
            {
                while (reader.ReadToFollowing()) { }
            }
            catch (Exception)
            {
                Assert.True(reader.IsInErrorState);
                throw;
            }
        }


        class NonReadableStream : MemoryStream
        {
            public override bool CanRead
            {
                get { return false; }
            }


            public override int ReadByte()
            {
                throw new NotSupportedException();
            }


            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }
}
