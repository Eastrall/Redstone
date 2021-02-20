using Microsoft.Extensions.Logging;
using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class TeleportConfirmHandler
    {
        private readonly ILogger<TeleportConfirmHandler> _logger;

        public TeleportConfirmHandler(ILogger<TeleportConfirmHandler> logger)
        {
            _logger = logger;
        }

        [PlayPacketHandler(ServerPlayPacketType.TeleportConfirm)]
        public void OnTeleportConfirm(MinecraftUser user, IMinecraftPacket packet)
        {
            int teleportId = packet.ReadVarInt32();

            _logger.LogInformation($"Teleport ID: {teleportId}");
        }
    }
}
