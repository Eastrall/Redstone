using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using System;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerRotationHandler
    {
        [PlayPacketHandler(ServerPlayPacketType.PlayerRotation)]
        public void OnPlayerRotation(IMinecraftUser user, IMinecraftPacket packet)
        {
            float yawAngle = packet.ReadSingle();
            float pitchAngle = packet.ReadSingle();
            bool isOnGround = packet.ReadBoolean();

            if (user.Player.IsOnGround != isOnGround)
            {
                throw new InvalidOperationException("Player is not on ground.");
            }

            user.Player.Rotate(yawAngle, pitchAngle);
        }
    }
}
