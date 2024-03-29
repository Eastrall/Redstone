﻿using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Handskake;
using Redstone.Protocol.Packets.Handskake.Server;

namespace Redstone.Server.Handlers;

public class HandshakeHandler
{
    [HandshakePacketHandler(ServerHandshakePacketType.Handshaking)]
    public void OnHandshake(IMinecraftUser user, IMinecraftPacket packet)
    {
        var handshake = new HandshakePacket(packet);

        user.UpdateStatus(handshake.NextState);
    }
}
