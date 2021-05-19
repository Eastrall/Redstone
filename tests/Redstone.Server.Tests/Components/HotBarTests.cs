using Moq;
using Redstone.Abstractions.Components;
using Redstone.Abstractions.Entities;
using Redstone.Common;
using Redstone.Server.Components;
using Xunit;

namespace Redstone.Server.Tests.Components
{
    public class HotBarTests
    {
        private readonly Mock<IPlayer> _mockedPlayer;
        private readonly IHotBar _hotBar;

        public HotBarTests()
        {
            _mockedPlayer = new Mock<IPlayer>();
            _hotBar = new HotBar(_mockedPlayer.Object);
        }

        [Fact]
        public void GetSelectedItemSlotWithEmptyHotBarTest()
        {
            IItemSlot itemSlot = _hotBar.SelectedSlot;

            Assert.NotNull(itemSlot);
            Assert.False(itemSlot.HasItem);
            Assert.Null(itemSlot.ItemId);
            Assert.Equal(0, itemSlot.ItemCount);
        }

        [Fact]
        public void SetSelectedIndexTest()
        {
            const int slotIndex = 3;
            const int itemId = 1;
            const int itemQuantity = 12;

            _hotBar.SetItem(slotIndex, itemId, itemQuantity);

            _hotBar.SetSlotIndex(slotIndex);

            Assert.NotNull(_hotBar.SelectedSlot);
            Assert.True(_hotBar.SelectedSlot.HasItem);
            Assert.Equal(itemId, _hotBar.SelectedSlot.ItemId);
            Assert.Equal(itemQuantity, _hotBar.SelectedSlot.ItemCount);

            _mockedPlayer.Verify(x => x.Equip(EquipementSlotType.MainHand, _hotBar.SelectedSlot), Times.Once());
        }
    }
}
