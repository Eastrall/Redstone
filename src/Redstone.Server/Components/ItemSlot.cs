using Redstone.Abstractions.Components;

namespace Redstone.Server.Components
{
    public class ItemSlot : IItemSlot
    {
        public bool HasItem => ItemId.HasValue;

        public int? ItemId { get; set; }

        public byte ItemCount { get; set; }

        public ItemSlot()
            : this(default, default)
        {
        }

        public ItemSlot(int itemId, byte itemCount)
        {
            ItemId = itemId;
            ItemCount = itemCount;
        }
    }
}
