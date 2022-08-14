using Redstone.Abstractions;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Events;
using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Packets.Game.Client;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server;

internal class RedstoneServerEvents : IRedstoneServerEvents
{
    private readonly IRedstoneServer _server;

    public RedstoneServerEvents(IRedstoneServer server)
    {
        _server = server;
    }

    public void OnPlayerJoinGame(PlayerJoinEventArgs playerJoinEvent)
    {
        IPlayer player = playerJoinEvent.Player;
        using var packet = new PlayerInfoPacket(PlayerInfoActionType.Add, player);

        IEnumerable<IMinecraftUser> players = _server.ConnectedPlayers.Where(x => x.Player.Id != player.Id);

        _server.SendTo(players, packet);
    }

    public void OnPlayerLeaveGame(PlayerLeaveEventArgs playerLeaveEvent)
    {
        IPlayer player = playerLeaveEvent.Player;
        using var packet = new PlayerInfoPacket(PlayerInfoActionType.Remove, player);
        using var removePlayerPacket = new DestroyEntitiesPacket(player);

        IEnumerable<IMinecraftUser> players = _server.ConnectedPlayers.Where(x => x.Player.Id != player.Id);

        _server.SendTo(players, packet);
        _server.SendTo(players, removePlayerPacket);
    }
}
