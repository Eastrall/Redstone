using Moq;
using Redstone.Abstractions.Components;
using Redstone.Abstractions.Entities;
using Redstone.Server.Components;

namespace Redstone.Server.Tests.Components;

public class InventoryTests
{
    private readonly Mock<IEntity> _mockedEntity;
    private readonly IInventory _inventory;

    public InventoryTests()
    {
        _mockedEntity = new Mock<IEntity>();
        _inventory = new Inventory(_mockedEntity.Object);
    }
}
