using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Server.Registry;
using Redstone.Server.Tests.Mocks;
using System;
using Xunit;

namespace Redstone.Server.Tests.Entities;

public class PlayerEntityTests
{
    private readonly Faker _faker = new();
    private readonly IRegistry _registry;
    private readonly IServiceProvider _serviceProvider;

    public PlayerEntityTests()
    {
        _registry = new DataRegistry(new Mock<ILogger<DataRegistry>>().Object);
        _registry.Load();
        _serviceProvider = new ServiceCollection()
            .AddSingleton(s => _registry)
            .BuildServiceProvider();
    }

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

    [Fact]
    public void PlayerLookAroundTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
        var otherPlayer = CreatePlayer("OtherPlayer");
        var map = WorldMapMock.Create("minecraft:test");

        player.Position.X = 4;
        player.Position.Z = 4;
        player.IsSpawned = true;
        player.IsVisible = true;
        otherPlayer.Position.X = 2;
        otherPlayer.Position.Z = 2;
        otherPlayer.IsSpawned = true;
        otherPlayer.IsVisible = true;

        map.AddRegion(0, 0);
        map.AddPlayer(player);
        map.AddPlayer(otherPlayer);

        Assert.Empty(player.VisibleEntities);

        player.LookAround();

        Assert.NotEmpty(player.VisibleEntities);
        Assert.Contains(otherPlayer, player.VisibleEntities);
        minecraftUser.Verify(x => x.Send(It.IsAny<SpawnPlayerPacket>()), Times.Once());
    }

    [Fact]
    public void PlayerLookAroundWithoutBeingSpawnedTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);

        player.IsSpawned = false;
        player.IsVisible = false;
        player.LookAround();

        // Nothing to check since it's a normal behavior.
    }

    [Fact]
    public void PlayerLookAroundWhileBeingInvisibleTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);

        player.IsSpawned = true;
        player.IsVisible = false;
        player.LookAround();

        // Nothing to check since it's a normal behavior.
    }

    [Fact]
    public void PlayerMoveTest()
    {
        var minecraftUser = new MinecraftUserMock(Guid.NewGuid());
        var otherMinecraftUserMock = new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        var player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);
        var destinationPosition = new Position(3, 3, 8);
        var otherPlayer = CreatePlayer("OtherPlayer", otherMinecraftUserMock);
        var map = WorldMapMock.Create("minecraft:test");
        IRegion region = map.AddRegion(0, 0);
        IChunk chunk = region.AddChunk(0, 0);

        Assert.NotNull(region);
        Assert.NotNull(chunk);
        map.AddPlayer(player);
        map.AddPlayer(otherPlayer);

        player.IsSpawned = true;
        player.IsVisible = true;
        otherPlayer.IsSpawned = true;
        otherPlayer.IsVisible = true;

        player.Position.X = 1;
        player.Position.Z = 1;

        player.LookAround();
        otherPlayer.LookAround();

        player.Move(destinationPosition, true);

        Assert.Equal(destinationPosition, player.Position);
        otherMinecraftUserMock.Verify(x => x.Send(It.IsAny<EntityPositionPacket>()), Times.Once());
    }

    private static IPlayer CreatePlayer(string name, MinecraftUserMock minecraftUserMock = null)
    {
        var minecraftUser = minecraftUserMock ?? new MinecraftUserMock(Guid.NewGuid());
        var worldMock = new WorldMock();
        IPlayer player = PlayerEntityGenerator.GeneratePlayer(minecraftUser, worldMock);

        player.SetName(name);

        return player;
    }
}
