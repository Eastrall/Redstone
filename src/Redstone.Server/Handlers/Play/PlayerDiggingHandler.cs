using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerDiggingHandler
    {
        private readonly ILogger<PlayerDiggingHandler> _logger;

        public PlayerDiggingHandler(ILogger<PlayerDiggingHandler> logger)
        {
            _logger = logger;
        }

        [PlayPacketHandler(ServerPlayPacketType.PlayerDigging)]
        public void OnPlayerDigging(IMinecraftUser user, IMinecraftPacket packet)
        {
            var status = (DiggingType)packet.ReadVarInt32();
            var position = packet.ReadPosition();
            var face = (BlockFaceType)packet.ReadByte();

            _logger.LogTrace($"Status={status};Position={position};Face={face}");

            if (user.Player.GameMode is ServerGameModeType.Creative)
            {
                user.Player.Map.SetBlock(BlockType.Air, (int)position.X, (int)position.Y, (int)position.Z);

            }
            else
            {
                // TODO: other modes
            }
        }
    }
}
