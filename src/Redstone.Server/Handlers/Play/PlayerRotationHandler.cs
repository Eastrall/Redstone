using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using Redstone.Protocol.Packets.Game.Client;

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

            user.Player.Angle = yawAngle;
            user.Player.HeadAngle = pitchAngle;

            using var entityRotationPacket = new EntityRotationPacket(
                user.Player.EntityId, 
                user.Player.Angle, 
                user.Player.HeadAngle, 
                isOnGround);

            using var entityHeadLookPacket = new EntityHeadLookPacket(user.Player.EntityId, user.Player.Angle);

            user.Player.SendPacketToVisibleEntities(entityRotationPacket);
            user.Player.SendPacketToVisibleEntities(entityHeadLookPacket);
        }
    }
}
