using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Status;
using Redstone.Protocol.Packets.Status.Client;
using Redstone.Protocol.Packets.Status.Server;

namespace Redstone.Server.Handlers.Status
{
    public class StatusPingHandler
    {
        [StatusPacketHandler(ServerStatusPacketType.Ping)]
        public void OnPing(MinecraftUser user, IMinecraftPacket packet)
        {
            var pingPacket = new StatusPingPacket(packet);
            using var pongPacket = new StatusPongPacket(pingPacket.Payload);
            user.Send(pongPacket);
        }
    }
}
