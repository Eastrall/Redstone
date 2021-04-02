using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Configuration;
using Redstone.Server.Entities;
using System;

namespace Redstone.Server.Tests.Mocks
{
    public static class PlayerEntityGenerator
    {
        private static readonly Faker _faker = new();

        public static IPlayer GeneratePlayer(Guid guid)
        {
            var minecraftUserMock = new Mock<IMinecraftUser>();
            minecraftUserMock.SetupGet(x => x.Id).Returns(guid);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IWorld))).Returns(new Mock<IWorld>().Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IOptions<GameConfiguration>))).Returns(new Mock<IOptions<GameConfiguration>>().Object);

            var player = new Player(minecraftUserMock.Object, Guid.NewGuid(), _faker.Name.FirstName(), serviceProviderMock.Object)
            {
                Angle = _faker.Random.Float(0, 360),
                HeadAngle = _faker.Random.Float(0, 360)
            };

            player.Position.Copy(new Position(_faker.Random.Float(), _faker.Random.Float(), _faker.Random.Float()));
            player.SetName(_faker.Name.FirstName());

            return player;
        }

        public static Mock<IPlayer> GeneratePlayerMock()
        {
            var playerMock = new Mock<IPlayer>();

            return playerMock;
        }
    }
}
