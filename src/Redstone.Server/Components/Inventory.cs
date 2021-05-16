using Redstone.Abstractions.Components;
using Redstone.Abstractions.Entities;

namespace Redstone.Server.Components
{
    internal class Inventory : ItemContainer, IInventory
    {
        private readonly IEntity _owner;

        public Inventory(IEntity owner) 
            : base(RedstoneContants.PlayerInventorySize)
        {
            _owner = owner;
        }
    }
}
