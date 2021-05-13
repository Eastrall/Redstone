using Redstone.Abstractions.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server.Components
{
    public class ItemContainer : IItemContainer
    {
        private readonly IReadOnlyCollection<IItemSlot> _itemsSlots;

        public int Capacity { get; }

        public int Count => _itemsSlots.Count(x => x.HasItem);

        public ItemContainer(int capacity)
        {
            Capacity = capacity;
            _itemsSlots = new List<IItemSlot>(Enumerable.Repeat<IItemSlot>(new ItemSlot(), capacity));
        }

        public IItemSlot GetItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= Capacity)
            {
                throw new IndexOutOfRangeException("The given index was out of range.");
            }

            return _itemsSlots.ElementAt(slotIndex);
        }

        public void SetItem(IItemSlot itemSlot, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= Capacity)
            {
                throw new IndexOutOfRangeException("The given index was out of range.");
            }

            throw new NotImplementedException();
        }

        public IEnumerator<IItemSlot> GetEnumerator() => _itemsSlots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _itemsSlots.GetEnumerator();
    }
}
