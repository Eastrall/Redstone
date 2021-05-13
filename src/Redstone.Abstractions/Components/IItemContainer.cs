using System.Collections.Generic;

namespace Redstone.Abstractions.Components
{
    public interface IItemContainer : IEnumerable<IItemSlot>
    {
        int Capacity { get; }

        int Count { get; }

        IItemSlot GetItem(int slotIndex);

        void SetItem(IItemSlot itemSlot, int slotIndex);
    }
}
