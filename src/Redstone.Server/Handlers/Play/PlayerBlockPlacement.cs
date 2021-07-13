using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Components;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Common;
using Redstone.Common.Structures.Blocks;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;
using Redstone.Protocol.Packets.Game.Client;
using System;
using System.Linq;

namespace Redstone.Server.Handlers.Play
{
    public class PlayerBlockPlacement
    {
        private readonly ILogger<PlayerBlockPlacement> _logger;
        private readonly IRegistry _registry;

        public PlayerBlockPlacement(ILogger<PlayerBlockPlacement> logger, IRegistry registry)
        {
            _logger = logger;
            _registry = registry;
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

            // TODO: check if the current player is interacting with an interactable object.
            // Like: Chest, anvil, crafting table.

            IItemSlot currentHeldItem = user.Player.HotBar.SelectedSlot;

            if (currentHeldItem.HasItem)
            {
                BlockData block = _registry.Blocks.FirstOrDefault(x => x.ItemId == currentHeldItem.ItemId);
                BlockType blockToPlace = block.Type;

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

                    _logger.LogDebug($"Placing block '{blockToPlace}' at position {realBlockPosition}");

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
