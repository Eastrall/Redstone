using Redstone.NBT.Tags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Redstone.NBT.Tests
{
    public sealed class CompoundTests
    {
        [Fact]
        public void InitializingCompoundFromCollectionTest()
        {
            NbtTag[] allNamed = {
                new NbtShort("allNamed1", 1),
                new NbtLong("allNamed2", 2),
                new NbtInt("allNamed3", 3)
            };

            NbtTag[] someUnnamed = {
                new NbtInt("someUnnamed1", 1),
                new NbtInt(2),
                new NbtInt("someUnnamed3", 3)
            };

            NbtTag[] someNull = {
                new NbtInt("someNull1", 1),
                null,
                new NbtInt("someNull3", 3)
            };

            NbtTag[] dupeNames = {
                new NbtInt("dupeNames1", 1),
                new NbtInt("dupeNames2", 2),
                new NbtInt("dupeNames1", 3)
            };

            // null collection, should throw
            Assert.Throws<ArgumentNullException>(() => new NbtCompound("nullTest", null));

            // proper initialization
            NbtCompound allNamedTest = null;
            allNamedTest = new NbtCompound("allNamedTest", allNamed);
            Assert.Equal(allNamed, allNamedTest);

            // some tags are unnamed, should throw
            Assert.Throws<ArgumentException>(() => new NbtCompound("someUnnamedTest", someUnnamed));

            // some tags are null, should throw
            Assert.Throws<ArgumentNullException>(() => new NbtCompound("someNullTest", someNull));

            // some tags have same names, should throw
            Assert.Throws<ArgumentException>(() => new NbtCompound("dupeNamesTest", dupeNames));
        }


        [Fact]
        public void GettersAndSetters()
        {
            // construct a document for us to test.
            var nestedChild = new NbtCompound("NestedChild");
            var nestedInt = new NbtInt(1);
            var nestedChildList = new NbtList("NestedChildList") {
                nestedInt
            };
            var child = new NbtCompound("Child") {
                nestedChild,
                nestedChildList
            };
            var childList = new NbtList("ChildList") {
                new NbtInt(1)
            };
            var parent = new NbtCompound("Parent") {
                child,
                childList
            };

            // Accessing nested compound tags using indexers
            Assert.Equal(nestedChild, parent["Child"]["NestedChild"]);
            Assert.Equal(nestedChildList, parent["Child"]["NestedChildList"]);
            Assert.Equal(nestedInt, parent["Child"]["NestedChildList"][0]);

            // Accessing nested compound tags using Get and Get<T>
            Assert.Throws<ArgumentNullException>(() => parent.Get<NbtCompound>(null));
            Assert.Null(parent.Get<NbtCompound>("NonExistingChild"));
            Assert.Equal(nestedChild, parent.Get<NbtCompound>("Child").Get<NbtCompound>("NestedChild"));
            Assert.Equal(nestedChildList, parent.Get<NbtCompound>("Child").Get<NbtList>("NestedChildList"));
            Assert.Equal(nestedInt, parent.Get<NbtCompound>("Child").Get<NbtList>("NestedChildList")[0]);
            Assert.Throws<ArgumentNullException>(() => parent.Get(null));
            Assert.Null(parent.Get("NonExistingChild"));
            Assert.Equal(nestedChild, (parent.Get("Child") as NbtCompound).Get("NestedChild"));
            Assert.Equal(nestedChildList, (parent.Get("Child") as NbtCompound).Get("NestedChildList"));
            Assert.Equal(nestedInt, (parent.Get("Child") as NbtCompound).Get("NestedChildList")[0]);

            // Accessing with Get<T> and an invalid given type
            Assert.Throws<InvalidCastException>(() => parent.Get<NbtInt>("Child"));

            // Using TryGet and TryGet<T>
            NbtTag dummyTag;
            Assert.Throws<ArgumentNullException>(() => parent.TryGet(null, out dummyTag));
            Assert.False(parent.TryGet("NonExistingChild", out dummyTag));
            Assert.True(parent.TryGet("Child", out dummyTag));
            NbtCompound dummyCompoundTag;
            Assert.Throws<ArgumentNullException>(() => parent.TryGet(null, out dummyCompoundTag));
            Assert.False(parent.TryGet("NonExistingChild", out dummyCompoundTag));
            Assert.True(parent.TryGet("Child", out dummyCompoundTag));

            // Trying to use integer indexers on non-NbtList tags
            Assert.Throws<InvalidOperationException>(() => parent[0] = nestedInt);
            Assert.Throws<InvalidOperationException>(() => nestedInt[0] = nestedInt);

            // Trying to use string indexers on non-NbtCompound tags
            Assert.Throws<InvalidOperationException>(() => childList["test"] = nestedInt);
            Assert.Throws<InvalidOperationException>(() => nestedInt["test"] = nestedInt);

            // Trying to get a non-existent element by name
            Assert.Null(parent.Get<NbtTag>("NonExistentTag"));
            Assert.Null(parent["NonExistentTag"]);

            // Null indices on NbtCompound
            Assert.Throws<ArgumentNullException>(() => parent.Get<NbtTag>(null));
            Assert.Throws<ArgumentNullException>(() => parent[null] = new NbtInt(1));
            Assert.Throws<ArgumentNullException>(() => nestedInt = (NbtInt)parent[null]);

            // Out-of-range indices on NbtList
            Assert.Throws<ArgumentOutOfRangeException>(() => nestedInt = (NbtInt)childList[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => childList[-1] = new NbtInt(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => nestedInt = childList.Get<NbtInt>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => nestedInt = (NbtInt)childList[childList.Count]);
            Assert.Throws<ArgumentOutOfRangeException>(() => nestedInt = childList.Get<NbtInt>(childList.Count));

            // Using setter correctly
            parent["NewChild"] = new NbtByte("NewChild");

            // Using setter incorrectly
            object dummyObject;
            Assert.Throws<ArgumentNullException>(() => parent["Child"] = null);
            Assert.NotNull(parent["Child"]);
            Assert.Throws<ArgumentException>(() => parent["Child"] = new NbtByte("NotChild"));
            Assert.Throws<InvalidOperationException>(() => dummyObject = parent[0]);
            Assert.Throws<InvalidOperationException>(() => parent[0] = new NbtByte("NewerChild"));

            // Try adding tag to self
            var selfTest = new NbtCompound("SelfTest");
            Assert.Throws<ArgumentException>(() => selfTest["SelfTest"] = selfTest);

            // Try adding a tag that already has a parent
            Assert.Throws<ArgumentException>(() => selfTest[child.Name] = child);
        }


        [Fact]
        public void Renaming()
        {
            var tagToRename = new NbtInt("DifferentName", 1);
            var compound = new NbtCompound {
                new NbtInt("SameName", 1),
                tagToRename
            };

            // proper renaming, should not throw
            tagToRename.Name = "SomeOtherName";

            // attempting to use a duplicate name
            Assert.Throws<ArgumentException>(() => tagToRename.Name = "SameName");

            // assigning a null name to a tag inside a compound; should throw
            Assert.Throws<ArgumentNullException>(() => tagToRename.Name = null);

            // assigning a null name to a tag that's been removed; should not throw
            compound.Remove(tagToRename);
            tagToRename.Name = null;
        }


        [Fact]
        public void AddingAndRemoving()
        {
            var foo = new NbtInt("Foo");
            var test = new NbtCompound {
                foo
            };

            // adding duplicate object
            Assert.Throws<ArgumentException>(() => test.Add(foo));

            // adding duplicate name
            Assert.Throws<ArgumentException>(() => test.Add(new NbtByte("Foo")));

            // adding unnamed tag
            Assert.Throws<ArgumentException>(() => test.Add(new NbtInt()));

            // adding null
            Assert.Throws<ArgumentNullException>(() => test.Add(null));

            // adding tag to self
            Assert.Throws<ArgumentException>(() => test.Add(test));

            // contains existing name/object
            Assert.True(test.Contains("Foo"));
            Assert.Contains(foo, test);
            Assert.Throws<ArgumentNullException>(() => test.Contains((string)null));
            Assert.Throws<ArgumentNullException>(() => test.Contains((NbtTag)null));

            // contains non-existent name
            Assert.False(test.Contains("Bar"));

            // contains existing name / different object
            Assert.DoesNotContain(new NbtInt("Foo"), test);

            // removing non-existent name
            Assert.Throws<ArgumentNullException>(() => test.Remove((string)null));
            Assert.False(test.Remove("Bar"));

            // removing existing name
            Assert.True(test.Remove("Foo"));

            // removing non-existent name
            Assert.False(test.Remove("Foo"));

            // re-adding object
            test.Add(foo);

            // removing existing object
            Assert.Throws<ArgumentNullException>(() => test.Remove((NbtTag)null));
            Assert.True(test.Remove(foo));
            Assert.False(test.Remove(foo));

            // clearing an empty NbtCompound
            Assert.Empty(test);
            test.Clear();

            // re-adding after clearing
            test.Add(foo);
            Assert.Single(test);

            // clearing a non-empty NbtCompound
            test.Clear();
            Assert.Empty(test);
        }


        [Fact]
        public void UtilityMethods()
        {
            NbtTag[] testThings = {
                new NbtShort("Name1", 1),
                new NbtInt("Name2", 2),
                new NbtLong("Name3", 3)
            };
            var compound = new NbtCompound();

            // add range
            compound.AddRange(testThings);

            // add range with duplicates
            Assert.Throws<ArgumentException>(() => compound.AddRange(testThings));
        }


        [Fact]
        public void InterfaceImplementations()
        {
            NbtTag[] tagList = {
                new NbtByte("First", 1), new NbtShort("Second", 2), new NbtInt("Third", 3),
                new NbtLong("Fourth", 4L)
            };

            // test NbtCompound(IEnumerable<NbtTag>) constructor
            var comp = new NbtCompound(tagList);

            // test .Names and .Tags collections
            Assert.Equal(new[] {
                "First", "Second", "Third", "Fourth"
            }, comp.Names);
            Assert.Equal(tagList, comp.Tags);

            // test ICollection and ICollection<NbtTag> boilerplate properties
            ICollection<NbtTag> iGenCollection = comp;
            Assert.False(iGenCollection.IsReadOnly);
            ICollection iCollection = comp;
            Assert.NotNull(iCollection.SyncRoot);
            Assert.False(iCollection.IsSynchronized);

            // test CopyTo()
            var tags = new NbtTag[iCollection.Count];
            iCollection.CopyTo(tags, 0);
            Assert.Equal(comp, tags);

            // test non-generic GetEnumerator()
            var enumeratedTags = comp.ToList();
            Assert.Equal(tagList, enumeratedTags);

            // test generic GetEnumerator()
            List<NbtTag> enumeratedTags2 = new List<NbtTag>();
            var enumerator = comp.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumeratedTags2.Add(enumerator.Current);
            }
            Assert.Equal(tagList, enumeratedTags2);
        }
    }
}
