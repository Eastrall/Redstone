using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Handskake;
using Redstone.Protocol.Packets.Handskake.Server;

namespace Redstone.Server.Handlers.Handshake
{
    public class HandshakeHandler
    {
        [HandshakePacketHandler(ServerHandshakePacketType.Handshaking)]
        public void OnHandshake(MinecraftUser user, IMinecraftPacket packet)
        {
            var handshake = new HandshakePacket(packet);

            user.Status = handshake.NextState;
        }
    }
}
