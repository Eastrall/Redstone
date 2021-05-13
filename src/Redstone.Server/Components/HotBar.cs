using Redstone.Abstractions.Components;

namespace Redstone.Server.Components
{
    public class HotBar : ItemContainer, IHotBar
    {
        public HotBar(int capacity)
            : base(capacity)
        {
        }
    }
}
