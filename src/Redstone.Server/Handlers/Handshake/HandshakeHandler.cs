using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Handskake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redstone.Server.Handlers.Handshake
{
    public class HandshakeHandler
    {
        [HandshakePacketHandler(ServerHandshakePacketType.Handshaking)]
        public void OnHandshake(MinecraftUser user, IMinecraftPacket packet)
        {
            // Process handshake
        }
    }
}
