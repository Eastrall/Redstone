using Redstone.Abstractions.Components;

namespace Redstone.Server.Components
{
    public class Inventory : ItemContainer, IInventory
    {
        public Inventory(int capacity) 
            : base(capacity)
        {
        }
    }
}
