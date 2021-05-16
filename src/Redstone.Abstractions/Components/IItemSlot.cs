namespace Redstone.Abstractions.Components
{
    public interface IItemSlot
    {
        /// <summary>
        /// Gets a boolean that indicates if the current slot has an item.
        /// </summary>
        bool HasItem { get; }

        /// <summary>
        /// Gets or sets the item id of the current slot.
        /// </summary>
        int? ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item count of the current slot.
        /// </summary>
        byte ItemCount { get; set; }

        /// <summary>
        /// Resets the item slot.
        /// </summary>
        void Reset();
    }
}
