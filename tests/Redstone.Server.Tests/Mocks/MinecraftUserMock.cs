using Moq;
using Redstone.Abstractions.Protocol;
using System;

namespace Redstone.Server.Tests.Mocks;

public class MinecraftUserMock : Mock<IMinecraftUser>
{
    public Guid Id { get; }

    public MinecraftUserMock(Guid id)
    {
        Id = id;
        SetupGet(x => x.Id).Returns(id);
    }
}
