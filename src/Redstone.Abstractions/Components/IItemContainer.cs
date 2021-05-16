using System.Collections.Generic;

namespace Redstone.Abstractions.Components
{
    public interface IItemContainer : IEnumerable<IItemSlot>
    {
        int Capacity { get; }

        int Count { get; }

        IItemSlot GetItem(int slotIndex);

        void SetItem(int slotIndex, int itemId, byte quantity = 1);

        void ClearItem(int slotIndex);
    }
}
