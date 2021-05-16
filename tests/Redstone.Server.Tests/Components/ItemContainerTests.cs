using Bogus;
using Redstone.Abstractions.Components;
using Redstone.Server.Components;
using System;
using Xunit;

namespace Redstone.Server.Tests.Components
{
    public class ItemContainerTests
    {
        private readonly Faker _faker;
        private readonly int _containerCapacity;
        
        private IItemContainer _itemContainer;

        public ItemContainerTests()
        {
            _faker = new Faker();
            _containerCapacity = _faker.Random.Int(10, 100);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateContainerWithInvalidCapacityTest(int containerCapacity)
        {
            Assert.Throws<ArgumentException>(() => new ItemContainer(containerCapacity));
        }

        [Fact]
        public void GetEmptyItemAtSlotTest()
        {
            CreateItemContainer();

            IItemSlot itemSlot = _itemContainer.GetItem(0);

            Assert.NotNull(itemSlot);
            Assert.Null(itemSlot.ItemId);
            Assert.Equal(0, itemSlot.ItemCount);
            Assert.False(itemSlot.HasItem);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void GetItemAtSlotWithInvalidIndexTest(int slotIndex)
        {
            CreateItemContainer();
            Assert.Throws<IndexOutOfRangeException>(() => _itemContainer.GetItem(slotIndex));
        }

        [Fact]
        public void SetItemAtSlotTest()
        {
            const int slotIndex = 0;
            const int itemId = 1;
            const int itemQuantity = 12;
            
            CreateItemContainer();
            _itemContainer.SetItem(slotIndex, itemId, itemQuantity);
            IItemSlot slot = _itemContainer.GetItem(slotIndex);

            Assert.NotNull(slot);
            Assert.True(slot.HasItem);
            Assert.Equal(itemId, slot.ItemId);
            Assert.Equal(itemQuantity, slot.ItemCount);
            Assert.Equal(1, _itemContainer.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SetItemAtSlotWithInvalidIndexTest(int slotIndex)
        {
            CreateItemContainer();
            Assert.Throws<IndexOutOfRangeException>(() => _itemContainer.SetItem(slotIndex, 1, 1));
        }

        [Fact]
        public void CountItemsInEmptyContainerTest()
        {
            CreateItemContainer();
            Assert.Equal(0, _itemContainer.Count);
        }

        [Fact]
        public void ClearItemAtSlotTest()
        {
            const int slotIndex = 0;
            const int itemId = 1;
            const int itemQuantity = 12;

            CreateItemContainer();

            _itemContainer.SetItem(slotIndex, itemId, itemQuantity);
            IItemSlot slot = _itemContainer.GetItem(slotIndex);

            Assert.NotNull(slot);
            Assert.True(slot.HasItem);

            _itemContainer.ClearItem(slotIndex);

            Assert.False(slot.HasItem);
            Assert.Null(slot.ItemId);
            Assert.Equal(0, slot.ItemCount);
            Assert.Equal(0, _itemContainer.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void ClearItemAtSlotWithInvalidIndex(int slotIndex)
        {
            CreateItemContainer();
            Assert.Throws<IndexOutOfRangeException>(() => _itemContainer.ClearItem(slotIndex));
        }

        [Fact]
        public void IterateOverEmptyContainerSlotsTest()
        {
            CreateItemContainer();

            foreach (IItemSlot slot in _itemContainer)
            {
                Assert.NotNull(slot);
                Assert.False(slot.HasItem);
                Assert.Null(slot.ItemId);
                Assert.Equal(0, slot.ItemCount);
            }
        }

        private void CreateItemContainer()
        {
            _itemContainer = new ItemContainer(_containerCapacity);
            Assert.Equal(_containerCapacity, _itemContainer.Capacity);
        }
    }
}
