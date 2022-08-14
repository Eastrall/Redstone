using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using Redstone.Protocol.Packets.Game.Client;
using System;

namespace Redstone.Server.Handlers.Play;

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
            IBlock previousBlock = user.Player.Map.GetBlock(position);

            if (previousBlock.IsAir)
            {
                throw new InvalidOperationException($"Cannot dig air blocks ({position})");
            }

            using var playerDiggingAck = new AcknowledgePlayerDiggingPacket(position, previousBlock, DiggingType.Started);
            user.Player.SendPacketToVisibleEntities(playerDiggingAck);
            
            using var blockChange = new BlockChangePacket(BlockType.Air, position);
            user.Player.SendPacketToVisibleEntities(blockChange);

            IBlock block = user.Player.Map.SetBlock(BlockType.Air, position);

            using var chunkPacket = new ChunkDataPacket(block.Chunk);
            user.Player.SendPacketToVisibleEntities(chunkPacket, includeEntity: true);
        }
        else
        {
            // TODO: other modes
        }
    }
}
