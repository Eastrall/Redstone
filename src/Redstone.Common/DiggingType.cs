namespace Redstone.Common
{
    /// <summary>
    /// Represents the different digging status of a player.
    /// </summary>
    public enum DiggingType
    {
        /// <summary>
        /// The player starts digging a block.
        /// </summary>
        Started,

        /// <summary>
        /// The player cancels the block digging process.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The player has finish digging a block.
        /// </summary>
        Finished,

        /// <summary>
        /// The player drops an entire item stack.
        /// </summary>
        DropItemStack,

        /// <summary>
        /// The players drop a single item.
        /// </summary>
        DropItem,

        /// <summary>
        /// The player has started eating food, pulling back arrows, using buckets, etc...
        /// </summary>
        ShootArrowOrFinishEating,

        /// <summary>
        /// The player swaps an item to the opposit hand.
        /// </summary>
        SwapItemHand
    }
}
