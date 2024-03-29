﻿namespace Redstone.Abstractions.Components;

public interface IHotBar : IItemContainer
{
    IItemSlot SelectedSlot { get; }

    void SetSlotIndex(int slotIndex);
}
