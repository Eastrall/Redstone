using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerBlockPlacement
    {
        [PlayPacketHandler(ServerPlayPacketType.PlayerBlockPlacement)]
        public void OnPlayerBlockPlacement(IMinecraftUser user, IMinecraftPacket packet)
        {
            var handType = (HandType)packet.ReadVarInt32();
            Position blockPosition = packet.ReadPosition();
            var diggingType = (DiggingType)packet.ReadVarInt32();
            float cursorX = packet.ReadSingle();
            float cursorY = packet.ReadSingle();
            float cursorZ = packet.ReadSingle();
            bool isInsideBlock = packet.ReadBoolean();

            
        }
    }
}
