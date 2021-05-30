using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class AnimationHandler
    {
        [PlayPacketHandler(ServerPlayPacketType.Animation)]
        public void OnAnimation(IMinecraftUser user, IMinecraftPacket packet)
        {
            var hand = (HandType)packet.ReadVarInt32();

            user.Player.SwingHand(hand);
        }
    }
}
