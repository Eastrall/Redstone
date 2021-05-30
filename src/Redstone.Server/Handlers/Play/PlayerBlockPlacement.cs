using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Components;
using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using Redstone.Protocol.Packets.Game.Client;
using System;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerBlockPlacement
    {
        private readonly ILogger<PlayerBlockPlacement> _logger;

        public PlayerBlockPlacement(ILogger<PlayerBlockPlacement> logger)
        {
            _logger = logger;
        }

        [PlayPacketHandler(ServerPlayPacketType.PlayerBlockPlacement)]
        public void OnPlayerBlockPlacement(IMinecraftUser user, IMinecraftPacket packet)
        {
            var handType = (HandType)packet.ReadVarInt32();
            Position blockPosition = packet.ReadPosition();
            var blockFace = (BlockFaceType)packet.ReadVarInt32();
            float cursorX = packet.ReadSingle();
            float cursorY = packet.ReadSingle();
            float cursorZ = packet.ReadSingle();
            bool isInsideBlock = packet.ReadBoolean();

            _logger.LogDebug($"Block position: {blockPosition}");

            // TODO: check if the current player is interacting with an interactable object.
            // Like: Chest, anvil, crafting table.

            IItemSlot currentHeldItem = user.Player.HotBar.SelectedSlot;

            if (currentHeldItem.HasItem)
            {
                BlockType blockToPlace = (BlockType)currentHeldItem.ItemId;

                if (blockToPlace is not BlockType.Air)
                {
                    Position realBlockPosition = blockFace switch
                    {
                        BlockFaceType.Bottom => new Position(blockPosition.X, blockPosition.Y - 1, blockPosition.Z),
                        BlockFaceType.Top => new Position(blockPosition.X, blockPosition.Y + 1, blockPosition.Z),
                        BlockFaceType.North => new Position(blockPosition.X, blockPosition.Y, blockPosition.Z - 1),
                        BlockFaceType.South => new Position(blockPosition.X, blockPosition.Y, blockPosition.Z + 1),
                        BlockFaceType.West => new Position(blockPosition.X - 1, blockPosition.Y, blockPosition.Z),
                        BlockFaceType.East => new Position(blockPosition.X + 1, blockPosition.Y, blockPosition.Z),
                        _ => throw new InvalidOperationException("Invalid block face type.")
                    };

                    user.Player.Map.SetBlock(blockToPlace, (int)realBlockPosition.X, (int)realBlockPosition.Y, (int)realBlockPosition.Z);

                    using var blockChangePacket = new BlockChangePacket(blockToPlace, blockPosition);
                    user.Player.SendPacketToVisibleEntities(blockChangePacket);

                    using var chunkDataPacket = new ChunkDataPacket(user.Player.Chunk);
                    user.Player.SendPacketToVisibleEntities(chunkDataPacket);
                }
            }
        }
    }
}
