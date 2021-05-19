using Redstone.Abstractions.Components;
using Redstone.Common;

namespace Redstone.Abstractions.Entities
{
    /// <summary>
    /// Provides an interface that describes the player behavior.
    /// </summary>
    public interface IPlayer : IEntity
    {
        /// <summary>
        /// Gets the player name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the player's ping in milliseconds.
        /// </summary>
        int Ping { get; }

        /// <summary>
        /// Gets the current player's game mode.
        /// </summary>
        ServerGameModeType GameMode { get; }

        /// <summary>
        /// Gets the current player's inventory.
        /// </summary>
        IInventory Inventory { get; }

        /// <summary>
        /// Gets the current player's hotbar.
        /// </summary>
        IHotBar HotBar { get; }

        /// <summary>
        /// Sets the current player name.
        /// </summary>
        /// <param name="newName">New name.</param>
        /// <param name="notifyOtherPlayers">
        /// Boolean value that indicates if the new name should be broadcasted to every player on the server.
        /// </param>
        void SetName(string newName, bool notifyOtherPlayers = false);

        /// <summary>
        /// Keeps the current player alive by sending a "keep-alive" packet.
        /// </summary>
        /// <returns>Keep-Alive id.</returns>
        long KeepAlive();

        /// <summary>
        /// Checks if the given keep-alive id is the same has the one sent by the current player.
        /// </summary>
        /// <param name="keepAliveId">Keep alive id.</param>
        void CheckKeepAlive(long keepAliveId);

        /// <summary>
        /// The current player speaks the given text and broadcast the message to every player on the world.
        /// </summary>
        /// <param name="text">Text that the player should speak/display on the chat.</param>
        void Speak(string text);

        /// <summary>
        /// Equips an item to the given equipement slot.
        /// </summary>
        /// <param name="slot">Equipement slot.</param>
        /// <param name="item">Item to equip.</param>
        void Equip(EquipementSlotType slot, IItemSlot item);
    }
}
