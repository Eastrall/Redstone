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
            if (capacity <= 0)
            {
                throw new ArgumentException($"Capacity cannot be less or equal to zero.");
            }

            Capacity = capacity;
            _itemsSlots = new List<IItemSlot>(Enumerable.Range(0, capacity).Select(_ => new ItemSlot()));
        }

        public virtual IItemSlot GetItem(int slotIndex)
        {
            ThrowIfOutOfRange(slotIndex);

            return _itemsSlots.ElementAt(slotIndex);
        }

        public virtual void SetItem(int slotIndex, int itemId, byte quantity = 1)
        {
            IItemSlot slot = GetItem(slotIndex);

            if (slot is not null)
            {
                slot.ItemId = itemId;
                slot.ItemCount = quantity;
            }
        }

        public virtual void ClearItem(int slotIndex)
        {
            IItemSlot slot = GetItem(slotIndex);

            if (slot is not null)
            {
                slot.Reset();
            }
        }

        public IEnumerator<IItemSlot> GetEnumerator() => _itemsSlots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _itemsSlots.GetEnumerator();

        private void ThrowIfOutOfRange(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= Capacity)
            {
                throw new IndexOutOfRangeException($"The given index '{slotIndex}' was out of range. Max capacity: '{Capacity}'.");
            }
        }
    }
}
