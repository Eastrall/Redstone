using Moq;
using Redstone.Abstractions.Components;
using Redstone.Abstractions.Entities;
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
    }
}
