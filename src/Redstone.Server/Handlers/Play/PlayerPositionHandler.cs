using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerPositionHandler
    {
        [PlayPacketHandler(ServerPlayPacketType.PlayerPosition)]
        public void OnPlayerPosition(IMinecraftUser user, IMinecraftPacket packet)
        {
            Position destinationPosition = packet.ReadAbsolutePosition();
            bool isOnGround = packet.ReadBoolean();

            user.Player.Move(destinationPosition, isOnGround);
        }
    }
}
