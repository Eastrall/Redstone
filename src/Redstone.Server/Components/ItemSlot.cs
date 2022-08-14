using Redstone.Abstractions.Components;

namespace Redstone.Server.Components;

public class ItemSlot : IItemSlot
{
    public bool HasItem => ItemId.HasValue;

    public int? ItemId { get; set; }

    public byte ItemCount { get; set; }

    // TODO: Item Metadata such as enchantments

    public ItemSlot()
    {
        ItemId = null;
        ItemCount = 0;
    }

    public ItemSlot(int itemId, byte itemCount)
    {
        ItemId = itemId;
        ItemCount = itemCount;
    }

    public void Reset()
    {
        ItemId = null;
        ItemCount = 0;
    }
}
