using Bogus;
using Moq;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Server.Tests.Mocks;
using System;
using Xunit;

namespace Redstone.Server.Tests.Entities
{
    public class PlayerEntityTests
    {
        private readonly Faker _faker = new();

        [Fact]
        public void PlayerSetName()
        {
            var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
            var worldMock = new WorldMock();
            var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
            string newName = _faker.Name.FirstName();

            player.SetName(newName, notifyOtherPlayers: true);

            Assert.Equal(newName, player.Name);
            // TODO: check notification
        }

        [Fact]
        public void PlayerKeepAlive()
        {
            var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
            var worldMock = new WorldMock();
            var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);

            long keepAliveId = player.KeepAlive();
            player.CheckKeepAlive(keepAliveId);

            minecraftUser.Verify(x => x.Send(It.IsAny<KeepAlivePacket>()), Times.Once());
            worldMock.Verify(x => x.SendToAll(It.IsAny<PlayerInfoPacket>()), Times.Once());
        }

        [Fact]
        public void PlayerCheckInvalidKeepAlive()
        {
            var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
            var worldMock = new WorldMock();
            var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);

            player.KeepAlive();
            player.CheckKeepAlive(int.MaxValue);

            minecraftUser.Verify(x => x.Send(It.IsAny<KeepAlivePacket>()), Times.Once());
            minecraftUser.Verify(x => x.Disconnect(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void PlayerSpeakTest()
        {
            var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
            var worldMock = new WorldMock();
            var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
            string textToSpeak = _faker.Lorem.Sentence();

            player.Speak(textToSpeak);

            worldMock.Verify(x => x.SendToAll(It.IsAny<ChatMessagePacket>()), Times.Once);
        }
    }
}
