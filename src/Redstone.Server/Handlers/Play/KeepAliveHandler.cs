using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class KeepAliveHandler
    {
        [PlayPacketHandler(ServerPlayPacketType.KeepAlive)]
        public void OnKeepAlive(MinecraftUser user, IMinecraftPacket packet)
        {
            long keepAliveId = packet.ReadInt64();

            user.Player.CheckKeepAlive(keepAliveId);
        }
    }
}
