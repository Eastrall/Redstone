using System;
using System.Collections.Generic;
using System.Text;

namespace Redstone.Protocol.Packets.Game.Client
{
    public class JoinGamePacket : MinecraftPacket
    {
        public JoinGamePacket()
            : base(ClientPlayPacketType.JoinGame)
        {
        }
    }
}
