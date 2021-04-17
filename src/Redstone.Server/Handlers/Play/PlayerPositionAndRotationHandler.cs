using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerPositionAndRotationHandler
    {
        [PlayPacketHandler(ServerPlayPacketType.PlayerPositionAndRotation)]
        public void OnPlayerPositionAndRotation(IMinecraftUser user, IMinecraftPacket packet)
        {
            Position destinationPosition = packet.ReadAbsolutePosition();
            float yawAngle = packet.ReadSingle();
            float pitchAngle = packet.ReadSingle();
            bool isOnGround = packet.ReadBoolean();

            user.Player.MoveAndRotate(destinationPosition, yawAngle, pitchAngle, isOnGround);
        }
    }
}
