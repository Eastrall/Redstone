using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Configuration;
using Redstone.Server.Entities;
using Redstone.Server.World;
using System;

namespace Redstone.Server.Tests.Mocks;

public static class PlayerEntityGenerator
{
    private static readonly Faker _faker = new();

    public static IPlayer GeneratePlayer(Mock<IMinecraftUser> user, Mock<IWorld> world = null)
    {
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(IWorld))).Returns(world?.Object ?? default);
        serviceProviderMock.Setup(x => x.GetService(typeof(IOptions<GameOptions>))).Returns(Options.Create<GameOptions>(new()));

        var player = new Player(user.Object, Guid.NewGuid(), _faker.Name.FirstName(), serviceProviderMock.Object)
        {
            Angle = _faker.Random.Float(0, 360),
            HeadAngle = _faker.Random.Float(0, 360)
        };

        player.Position.Copy(new Position(_faker.Random.Float(), _faker.Random.Float(), _faker.Random.Float()));
        player.SetName(_faker.Name.FirstName());

        return player;
    }

    public static Mock<IPlayer> GeneratePlayerMock(Mock<IWorld> world = null)
    {
        var playerMock = new Mock<IPlayer>();

        playerMock.SetupGet(x => x.World).Returns(world?.Object ?? default);

        return playerMock;
    }
}
