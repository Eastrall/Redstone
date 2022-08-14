using Redstone.Abstractions;
using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Status;
using Redstone.Protocol.Packets.Status.Client;

namespace Redstone.Server.Handlers.Status;

public class StatusRequestHandler
{
    private readonly IRedstoneServer _server;

    public StatusRequestHandler(IRedstoneServer server)
    {
        _server = server;
    }

    [StatusPacketHandler(ServerStatusPacketType.Request)]
    public void OnStatusRequest(IMinecraftUser user, IMinecraftPacket _)
    {
        using var responsePacket = new StatusResponsePacket(_server.GetServerStatus());
        
        user.Send(responsePacket);
    }
}
