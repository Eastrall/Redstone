using Redstone.NBT.Exceptions;
using Redstone.NBT.Tags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Redstone.NBT.Tests
{
    public sealed class ListTests
    {
        [Fact]
        public void InterfaceImplementation()
        {
            // prepare our test lists
            var referenceList = new List<NbtTag> {
                new NbtInt(1),
                new NbtInt(2),
                new NbtInt(3)
            };
            var testTag = new NbtInt(4);
            var originalList = new NbtList(referenceList);

            // check IList implementation
            IList iList = originalList;
            Assert.Equal(referenceList, iList);

            // check IList<NbtTag> implementation
            IList<NbtTag> iGenericList = originalList;
            Assert.Equal(referenceList, iGenericList);
            Assert.False(iGenericList.IsReadOnly);

            // check IList.Add
            referenceList.Add(testTag);
            iList.Add(testTag);
            Assert.Equal(referenceList, iList);

            // check IList.IndexOf
            Assert.Equal(referenceList.IndexOf(testTag), iList.IndexOf(testTag));
            Assert.True(iList.IndexOf(null) < 0);

            // check IList<NbtTag>.IndexOf
            Assert.Equal(referenceList.IndexOf(testTag), iGenericList.IndexOf(testTag));
            Assert.True(iGenericList.IndexOf(null) < 0);

            // check IList.Contains
            Assert.True(iList.Contains(testTag));
            Assert.False(iList.Contains(null));

            // check IList.Remove
            iList.Remove(testTag);
            Assert.False(iList.Contains(testTag));

            // check IList.Insert
            iList.Insert(0, testTag);
            Assert.Equal(0, iList.IndexOf(testTag));

            // check IList.RemoveAt
            iList.RemoveAt(0);
            Assert.False(iList.Contains(testTag));

            // check misc IList properties
            Assert.False(iList.IsFixedSize);
            Assert.False(iList.IsReadOnly);
            Assert.False(iList.IsSynchronized);
            Assert.NotNull(iList.SyncRoot);

            // check IList.CopyTo
            var exportTest = new NbtInt[iList.Count];
            iList.CopyTo(exportTest, 0);
            Assert.Equal(iList, exportTest);

            // check IList.this[int]
            for (int i = 0; i < iList.Count; i++)
            {
                Assert.Equal(originalList[i], iList[i]);
                iList[i] = new NbtInt(i);
            }

            // check IList.Clear
            iList.Clear();
            Assert.Equal(0, iList.Count);
            //Assert.Less(iList.IndexOf(testTag), 0);
        }


        [Fact]
        public void IndexerTest()
        {
            NbtByte ourTag = new NbtByte(1);
            var secondList = new NbtList {
                new NbtByte()
            };

            var testList = new NbtList();
            // Trying to set an out-of-range element
            Assert.Throws<ArgumentOutOfRangeException>(() => testList[0] = new NbtByte(1));

            // Make sure that setting did not affect ListType
            Assert.Equal(NbtTagType.Unknown, testList.ListType);
            Assert.Empty(testList);
            testList.Add(ourTag);

            // set a tag to null
            Assert.Throws<ArgumentNullException>(() => testList[0] = null);

            // set a tag to itself
            Assert.Throws<ArgumentException>(() => testList[0] = testList);

            // give a named tag where an unnamed tag was expected
            Assert.Throws<ArgumentException>(() => testList[0] = new NbtByte("NamedTag"));

            // give a tag of wrong type
            Assert.Throws<ArgumentException>(() => testList[0] = new NbtInt(0));

            // give an unnamed tag that already has a parent
            Assert.Throws<ArgumentException>(() => testList[0] = secondList[0]);

            // Make sure that none of the failed insertions went through
            Assert.Equal(ourTag, testList[0]);
        }


        [Fact]
        public void InitializingListFromCollection()
        {
            // auto-detecting list type
            var test1 = new NbtList("Test1", new NbtTag[] {
                new NbtInt(1),
                new NbtInt(2),
                new NbtInt(3)
            });
            Assert.Equal(NbtTagType.Int, test1.ListType);

            // check pre-conditions
            Assert.Throws<ArgumentNullException>(() => new NbtList((NbtTag[])null));
            Assert.Throws<ArgumentNullException>(() => new NbtList(null, null));
            new NbtList((string)null, NbtTagType.Unknown);
            Assert.Throws<ArgumentNullException>(() => new NbtList((NbtTag[])null, NbtTagType.Unknown));

            // correct explicitly-given list type
            new NbtList("Test2", new NbtTag[] {
                new NbtInt(1),
                new NbtInt(2),
                new NbtInt(3)
            }, NbtTagType.Int);

            // wrong explicitly-given list type
            Assert.Throws<ArgumentException>(() => new NbtList("Test3", new NbtTag[] {
                new NbtInt(1),
                new NbtInt(2),
                new NbtInt(3)
            }, NbtTagType.Float));

            // auto-detecting mixed list given
            Assert.Throws<ArgumentException>(() => new NbtList("Test4", new NbtTag[] {
                new NbtFloat(1),
                new NbtByte(2),
                new NbtInt(3)
            }));

            // using AddRange
            new NbtList().AddRange(new NbtTag[] {
                new NbtInt(1),
                new NbtInt(2),
                new NbtInt(3)
            });
            Assert.Throws<ArgumentNullException>(() => new NbtList().AddRange(null));
        }


        [Fact]
        public void ManipulatingList()
        {
            var sameTags = new NbtTag[] {
                new NbtInt(0),
                new NbtInt(1),
                new NbtInt(2)
            };

            var list = new NbtList("Test1", sameTags);

            // testing enumerator, indexer, Contains, and IndexOf
            int j = 0;
            foreach (NbtTag tag in list)
            {
                Assert.Contains(sameTags[j], list);
                Assert.Equal(sameTags[j], tag);
                Assert.Equal(j, list.IndexOf(tag));
                j++;
            }

            // adding an item of correct type
            list.Add(new NbtInt(3));
            list.Insert(3, new NbtInt(4));

            // adding an item of wrong type
            Assert.Throws<ArgumentException>(() => list.Add(new NbtString()));
            Assert.Throws<ArgumentException>(() => list.Insert(3, new NbtString()));
            Assert.Throws<ArgumentNullException>(() => list.Insert(3, null));

            // testing array contents
            for (int i = 0; i < sameTags.Length; i++)
            {
                Assert.Same(sameTags[i], list[i]);
                Assert.Equal(i, ((NbtInt)list[i]).Value);
            }

            // test removal
            Assert.False(list.Remove(new NbtInt(5)));
            Assert.True(list.Remove(sameTags[0]));
            Assert.Throws<ArgumentNullException>(() => list.Remove(null));
            list.RemoveAt(0);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(10));

            // Test some failure scenarios for Add:
            // adding a list to itself
            var loopList = new NbtList();
            Assert.Equal(NbtTagType.Unknown, loopList.ListType);
            Assert.Throws<ArgumentException>(() => loopList.Add(loopList));

            // adding same tag to multiple lists
            Assert.Throws<ArgumentException>(() => loopList.Add(list[0]));
            Assert.Throws<ArgumentException>(() => loopList.Insert(0, list[0]));

            // adding null tag
            Assert.Throws<ArgumentNullException>(() => loopList.Add(null));

            // make sure that all those failed adds didn't affect the tag
            Assert.Empty(loopList);
            Assert.Equal(NbtTagType.Unknown, loopList.ListType);

            // try creating a list with invalid tag type
            Assert.Throws<ArgumentOutOfRangeException>(() => new NbtList((NbtTagType)200));
        }


        [Fact]
        public void ChangingListTagType()
        {
            var list = new NbtList();

            // changing list type to an out-of-range type
            Assert.Throws<ArgumentOutOfRangeException>(() => list.ListType = (NbtTagType)200);

            // failing to add or insert a tag should not change ListType
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-1, new NbtInt()));
            Assert.Throws<ArgumentException>(() => list.Add(new NbtInt("namedTagWhereUnnamedIsExpected")));
            Assert.Equal(NbtTagType.Unknown, list.ListType);

            // changing the type of an empty list to "End" is allowed, see https://github.com/fragmer/fNbt/issues/12
            list.ListType = NbtTagType.End;
            Assert.Equal(NbtTagType.End, list.ListType);

            // changing the type of an empty list back to "Unknown" is allowed too!
            list.ListType = NbtTagType.Unknown;
            Assert.Equal(NbtTagType.Unknown, list.ListType);

            // adding the first element should set the tag type
            list.Add(new NbtInt());
            Assert.Equal(NbtTagType.Int, list.ListType);

            // setting correct type for a non-empty list
            list.ListType = NbtTagType.Int;

            // changing list type to an incorrect type
            Assert.Throws<ArgumentException>(() => list.ListType = NbtTagType.Short);

            // after the list is cleared, we should once again be allowed to change its TagType
            list.Clear();
            list.ListType = NbtTagType.Short;
        }


        [Fact]
        public void SerializingWithoutListType()
        {
            var root = new NbtCompound("root") {
                new NbtList("list")
            };
            var file = new NbtFile(root);

            using var ms = new MemoryStream();
            
            // list should throw NbtFormatException, because its ListType is Unknown
            Assert.Throws<NbtFormatException>(() => file.SaveToStream(ms, NbtCompression.None));
        }


        [Fact]
        public void Serializing1()
        {
            // check the basics of saving/loading
            const NbtTagType expectedListType = NbtTagType.Int;
            const int elements = 10;

            // construct nbt file
            var writtenFile = new NbtFile(new NbtCompound("ListTypeTest"));
            var writtenList = new NbtList("Entities", null, expectedListType);
            for (int i = 0; i < elements; i++)
            {
                writtenList.Add(new NbtInt(i));
            }
            writtenFile.RootTag.Add(writtenList);

            // test saving
            byte[] data = writtenFile.SaveToBuffer(NbtCompression.None);

            // test loading
            var readFile = new NbtFile();
            long bytesRead = readFile.LoadFromBuffer(data, 0, data.Length, NbtCompression.None);
            Assert.Equal(bytesRead, data.Length);

            // check contents of loaded file
            Assert.NotNull(readFile.RootTag);
            Assert.IsType<NbtList>(readFile.RootTag["Entities"]);
            var readList = (NbtList)readFile.RootTag["Entities"];
            Assert.Equal(writtenList.ListType, readList.ListType);
            Assert.Equal(readList.Count, writtenList.Count);

            // check .ToArray
            Assert.Equal(readList, readList.ToArray());
            Assert.Equal(readList, readList.ToArray<NbtInt>());

            // check contents of loaded list
            for (int i = 0; i < elements; i++)
            {
                Assert.Equal(readList.Get<NbtInt>(i).Value, writtenList.Get<NbtInt>(i).Value);
            }
        }


        [Fact]
        public void Serializing2()
        {
            // check saving/loading lists of all possible value types
            var testFile = new NbtFile(TestFiles.MakeListTest());
            byte[] buffer = testFile.SaveToBuffer(NbtCompression.None);
            long bytesRead = testFile.LoadFromBuffer(buffer, 0, buffer.Length, NbtCompression.None);
            Assert.Equal(bytesRead, buffer.Length);
        }


        [Fact]
        public void SerializingEmpty()
        {
            // check saving/loading lists of all possible value types
            var testFile = new NbtFile(new NbtCompound("root") {
                new NbtList("emptyList", NbtTagType.End),
                new NbtList("listyList", NbtTagType.List) {
                    new NbtList(NbtTagType.End)
                }
            });
            byte[] buffer = testFile.SaveToBuffer(NbtCompression.None);

            testFile.LoadFromBuffer(buffer, 0, buffer.Length, NbtCompression.None);

            NbtList list1 = testFile.RootTag.Get<NbtList>("emptyList");
            Assert.Empty(list1);
            Assert.Equal(NbtTagType.End, list1.ListType);

            NbtList list2 = testFile.RootTag.Get<NbtList>("listyList");
            Assert.Single(list2);
            Assert.Equal(NbtTagType.List, list2.ListType);
            Assert.Empty(list2.Get<NbtList>(0));
            Assert.Equal(NbtTagType.End, list2.Get<NbtList>(0).ListType);
        }


        [Fact]
        public void NestedListAndCompoundTest()
        {
            byte[] data;
            {
                var root = new NbtCompound("Root");
                var outerList = new NbtList("OuterList", NbtTagType.Compound);
                var outerCompound = new NbtCompound();
                var innerList = new NbtList("InnerList", NbtTagType.Compound);
                var innerCompound = new NbtCompound();

                innerList.Add(innerCompound);
                outerCompound.Add(innerList);
                outerList.Add(outerCompound);
                root.Add(outerList);

                var file = new NbtFile(root);
                data = file.SaveToBuffer(NbtCompression.None);
            }
            {
                var file = new NbtFile();
                long bytesRead = file.LoadFromBuffer(data, 0, data.Length, NbtCompression.None);
                Assert.Equal(bytesRead, data.Length);
                Assert.Single(file.RootTag.Get<NbtList>("OuterList"));
                Assert.Null(file.RootTag.Get<NbtList>("OuterList").Get<NbtCompound>(0).Name);
                Assert.Single(file.RootTag.Get<NbtList>("OuterList")
                                    .Get<NbtCompound>(0)
                                    .Get<NbtList>("InnerList"));
                Assert.Null(file.RootTag.Get<NbtList>("OuterList")
                                    .Get<NbtCompound>(0)
                                    .Get<NbtList>("InnerList")
                                    .Get<NbtCompound>(0)
                                    .Name);
            }
        }


        [Fact]
        public void FirstInsertTest()
        {
            NbtList list = new NbtList();
            Assert.Equal(NbtTagType.Unknown, list.ListType);
            list.Insert(0, new NbtInt(123));
            // Inserting a tag should set ListType
            Assert.Equal(NbtTagType.Int, list.ListType);
        }
    }
}
