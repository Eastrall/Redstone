using Bogus;
using Moq;
using Redstone.Abstractions.Entities;
using Redstone.Common;
using Redstone.Protocol.Abstractions;
using Redstone.Server.Entities;
using System;

namespace Redstone.Server.Tests.Mocks
{
    public static class PlayerEntityGenerator
    {
        private static readonly Faker _faker = new Faker();

        public static IPlayer GeneratePlayer(Guid guid)
        {
            var minecraftUserMock = new Mock<IMinecraftUser>();
            minecraftUserMock.SetupGet(x => x.Id).Returns(guid);

            var player = new Player(minecraftUserMock.Object)
            {
                Angle = _faker.Random.Float(0, 360),
                HeadAngle = _faker.Random.Float(0, 360)
            };

            player.Position.Copy(new Position(_faker.Random.Float(), _faker.Random.Float(), _faker.Random.Float()));
            player.SetName(_faker.Name.FirstName());

            return player;
        }
    }
}
