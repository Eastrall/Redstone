using Redstone.Abstractions.Components;
using Redstone.Abstractions.Entities;
using Redstone.Common;

namespace Redstone.Server.Components
{
    internal class HotBar : ItemContainer, IHotBar
    {
        private readonly IPlayer _owner;
        private int _selectedSlotIndex;

        public IItemSlot SelectedSlot => GetItem(_selectedSlotIndex);

        public HotBar(IPlayer owner)
            : base(RedstoneContants.PlayerHotBarSize)
        {
            _selectedSlotIndex = 0;
            _owner = owner;
        }

        public void SetSlotIndex(int slotIndex)
        {
            ThrowIfOutOfRange(slotIndex);

            _selectedSlotIndex = slotIndex;
            _owner.Equip(EquipementSlotType.MainHand, SelectedSlot);
        }
    }
}
