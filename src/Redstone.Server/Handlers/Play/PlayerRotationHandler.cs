using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using System;

namespace Redstone.Server.Handlers.Play;

public class PlayerRotationHandler
{
    [PlayPacketHandler(ServerPlayPacketType.PlayerRotation)]
    public void OnPlayerRotation(IMinecraftUser user, IMinecraftPacket packet)
    {
        float yawAngle = packet.ReadSingle();
        float pitchAngle = packet.ReadSingle();
        bool isOnGround = packet.ReadBoolean();

        user.Player.Rotate(yawAngle, pitchAngle);
    }
}
