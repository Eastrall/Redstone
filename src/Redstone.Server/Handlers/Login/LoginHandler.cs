﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Configuration;
using Redstone.Common.Structures.Biomes;
using Redstone.Common.Structures.Dimensions;
using Redstone.NBT;
using Redstone.NBT.Serialization;
using Redstone.NBT.Tags;
using Redstone.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Protocol.Packets.Login;
using Redstone.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server.Handlers.Login
{
    public class LoginHandler
    {
        private readonly ILogger<LoginHandler> _logger;
        private readonly IOptions<ServerConfiguration> _serverConfiguration;
        private readonly IOptions<GameConfiguration> _gameConfiguration;
        private readonly IRegistry _registry;
        private readonly IRedstoneServer _server;
        private readonly IBlockFactory _blockFactory;
        private readonly IServiceProvider _serviceProvider;

        public LoginHandler(ILogger<LoginHandler> logger, IOptions<ServerConfiguration> serverConfiguration, IOptions<GameConfiguration> gameConfiguration, IRegistry registry, IRedstoneServer server, IBlockFactory blockFactory, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serverConfiguration = serverConfiguration;
            _gameConfiguration = gameConfiguration;
            _registry = registry;
            _server = server;
            _blockFactory = blockFactory;
            _serviceProvider = serviceProvider;
        }

        [LoginPacketHandler(ServerLoginPacketType.LoginStart)]
        public void OnLogin(MinecraftUser user, IMinecraftPacket packet)
        {
            user.Username = packet.ReadString();

            _logger.LogInformation($"{user.Username} trying to log-in");

            user.Player.SetName(user.Username);
            user.Player.Position.X = 8;
            user.Player.Position.Y = 2;
            user.Player.Position.Z = 8;
            // TODO: initialize current player
            // TODO: Read player data from storage (DB or file-system)

            if (_serverConfiguration.Value.Mode == ServerModeType.Offline)
            {
                SendLoginSucess(user);
                user.Status = MinecraftUserStatus.Play;
                SendJoinGame(user);
                SendServerBrand(user);
                // TODO: held item changed
                // TODO: declare recipes
                // TODO: Tags
                // TODO: Entity status
                // TODO: declare commands
                // TODO: Unlock recipes
                SendPlayerPositionAndLook(user, user.Player.Position);
                SendPlayerInfo(user, PlayerInfoActionType.Add); 
                SendPlayerInfo(user, PlayerInfoActionType.UpdateLatency);
                SendUpdateViewPosition(user);
                // TODO: Update light
                SendChunkData(user);
                SendUpdateViewPosition(user);
                // TODO: World border
                SendSpawnPosition(user, Position.Zero);
                SendPlayerPositionAndLook(user, user.Player.Position);
            }
            else
            {
                // TODO: login to Mojang API
            }
        }

        private void SendLoginSucess(MinecraftUser user)
        {
            using var p = new MinecraftPacket(ClientLoginPacketType.LoginSuccess);

            p.WriteUUID(user.Id);
            p.WriteString(user.Username);

            user.Send(p);
        }

        private void SendJoinGame(MinecraftUser user)
        {
            using var joinPacket = new JoinGamePacket();

            joinPacket.WriteInt32(user.Player.EntityId); // EntityID
            joinPacket.WriteBoolean(_gameConfiguration.Value.IsHardcore); // Is hardcore
            joinPacket.WriteByte((byte)_gameConfiguration.Value.Mode); // GameMode
            joinPacket.WriteSByte((sbyte)ServerGameModeType.Survival); // Previous game mode

            var worldList = new[] { "minecraft:world" };

            joinPacket.WriteVarInt32(worldList.Length); // World count
            foreach (string world in worldList)
            {
                joinPacket.WriteString(world);
            }

            WriteDimensionsAndBiomes(_registry.Dimensions, _registry.Biomes, joinPacket);

            Dimension currentDimension = _registry.Dimensions.First();
            WriteDimension(currentDimension, joinPacket);
            joinPacket.WriteString(currentDimension.Name); // World name identifier

            joinPacket.WriteInt64(_gameConfiguration.Value.Seed); // Seed
            joinPacket.WriteVarInt32((int)_serverConfiguration.Value.MaxPlayers); // Max players
            joinPacket.WriteVarInt32(Math.Clamp(_gameConfiguration.Value.RenderingDistance, RedstoneContants.MinimumRenderDistance, RedstoneContants.MaximumRenderDistance)); // Render distance (2-32 chunks)
            joinPacket.WriteBoolean(_serverConfiguration.Value.ReducedDebugInfo); // Reduced debug info
            joinPacket.WriteBoolean(_gameConfiguration.Value.DisplayRespawnScreen); // Respawn screen
            joinPacket.WriteBoolean(_serverConfiguration.Value.Debug); // Is debug
            joinPacket.WriteBoolean(_serverConfiguration.Value.FlatTerrain); // is flat terrain

            user.Send(joinPacket);
        }

        private void SendServerBrand(MinecraftUser user)
        {
            using var serverBrandPacket = new PluginMessagePacket("minecraft:brand");

            serverBrandPacket.WriteString("redstone");

            user.Send(serverBrandPacket);
        }

        private void SendPlayerInfo(MinecraftUser user, PlayerInfoActionType actionType)
        {
            using var packet = new PlayerInfoPacket();

            packet.WriteVarInt32((int)actionType);
            packet.WriteVarInt32((int)_server.ConnectedPlayersCount);

            foreach (MinecraftUser connectedPlayer in _server.ConnectedPlayers.Where(x => x.Status == MinecraftUserStatus.Play))
            {
                packet.WriteUUID(connectedPlayer.Id);

                if (actionType == PlayerInfoActionType.Add)
                {
                    packet.WriteString(connectedPlayer.Username);
                    packet.WriteVarInt32(0); // Optional properties
                    packet.WriteVarInt32((int)_gameConfiguration.Value.Mode);
                    packet.WriteVarInt32(0); // ping
                    packet.WriteBoolean(false); // Has display name

                    // if has display name then
                    // packet.WriteString(DisplayName);
                }
                else if (actionType == PlayerInfoActionType.UpdateLatency)
                {
                    packet.WriteVarInt32(0); // ping
                }
            }

            user.Send(packet);
        }

        private void SendUpdateViewPosition(MinecraftUser user)
        {
            // TODO: get chunk position according to current user's position.
            using var packet = new UpdateViewPositionPacket(0, 0);

            user.Send(packet);
        }

        private void SendChunkData(MinecraftUser user)
        {
            var world = new WorldMap("minecraft:overworld", _serviceProvider);
            IRegion region = world.AddRegion(0, 0);
            IChunk chunk = region.AddChunk(0, 0);

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    var block = _blockFactory.CreateBlock(BlockType.Dirt);
                    chunk.SetBlock(block, x, 1, z);
                }
            }

            chunk.GenerateHeightMap();

            world.StartUpdate();
            world.AddPlayer(user.Player);

            bool fullChunk = true;

            using var packet = new ChunkDataPacket();

            packet.WriteInt32(chunk.X); // Chunk X
            packet.WriteInt32(chunk.Z); // Chunk Z
            packet.WriteBoolean(fullChunk); // full chunk

            int mask = 0;

            // if full chunk
            using var chunkStream = new MinecraftPacket();
            for (int i = 0; i < chunk.Sections.Count(); i++)
            {
                var section = chunk.Sections.ElementAt(i) as ChunkSection;

                if (fullChunk)
                {
                    mask |= 1 << i;
                    section.Serialize(chunkStream);
                }
            }

            packet.WriteVarInt32(mask);

            //// Heightmap serialization
            ////var heightmapCompound = new NbtCompound("")
            ////{
            ////    new NbtLongArray("MOTION_BLOCKING", chunk.Heightmap.ToArray()),
            ////    new NbtLongArray("WORLD_SURFACE", chunk.WorldSurfaceHeightmap.ToArray())
            ////};
            ////var nbtFile = new NbtFile(heightmapCompound);


            var writer = new NbtWriter(packet, "");
            writer.WriteLongArray("MOTION_BLOCKING", chunk.Heightmap.ToArray());
            //writer.WriteLongArray("OCEAN_FLOOR", chunk.Heightmaps[HeightmapType.OceanFloor].data.Storage.Cast<long>().ToArray());
            writer.WriteLongArray("WORLD_SURFACE", chunk.WorldSurfaceHeightmap.ToArray());
            writer.EndCompound();
            writer.Finish();

            //packet.WriteBytes(nbtFile.GetBuffer());

            // Biomes
            if (fullChunk)
            {
                packet.WriteVarInt32(1024);
                for (int i = 0; i < 1024; i++)
                {
                    packet.WriteVarInt32(0);
                }
            }

            chunkStream.Position = 0;

            packet.WriteVarInt32((int)chunkStream.Length);
            packet.WriteBytes(chunkStream.BaseBuffer);

            packet.WriteVarInt32(0); // block count
            // TODO: foreach block in blocks in chunk as NBT

            user.Send(packet);
        }

        private void SendSpawnPosition(MinecraftUser user, Position position)
        {
            using var packet = new SpawnPositionPacket(position);

            user.Send(packet);
        }

        private void SendPlayerPositionAndLook(MinecraftUser user, Position position)
        {
            using var packet = new PlayerPositionAndLookPacket();

            packet.WriteDouble(position.X); // x
            packet.WriteDouble(position.Y); // y
            packet.WriteDouble(position.Z); // z
            packet.WriteSingle(0); // yaw
            packet.WriteSingle(0); // pitch
            packet.WriteByte(0);
            //packet.WriteByte(0x01 | 0x02 | 0x04); // position flags (x|y|z)
            packet.WriteVarInt32(0); // teleport id

            user.Send(packet);
        }

        private void WriteDimensionsAndBiomes(IEnumerable<Dimension> dimensions, IEnumerable<Biome> biomes, IMinecraftPacket packet)
        {
            IEnumerable<NbtTag> dimensionsTags = dimensions.Select(x => NbtSerializer.SerializeCompound(x));
            IEnumerable<NbtTag> biomesTags = biomes.Select(x => NbtSerializer.SerializeCompound(x));

            var nbtCompound = new NbtCompound("")
            {
                new NbtCompound("minecraft:dimension_type")
                {
                    new NbtString("type", "minecraft:dimension_type"),
                    new NbtList("value", dimensionsTags, NbtTagType.Compound)
                },
                new NbtCompound("minecraft:worldgen/biome")
                {
                    new NbtString("type", "minecraft:worldgen/biome"),
                    new NbtList("value", biomesTags, NbtTagType.Compound)
                }
            };
            var nbtFile = new NbtFile(nbtCompound);

            packet.WriteBytes(nbtFile.GetBuffer());
        }

        private void WriteDimension(Dimension dimension, IMinecraftPacket packet)
        {
            var nbtDimension = NbtSerializer.SerializeCompound(dimension.Element, "");
            var nbtFile = new NbtFile(nbtDimension);

            packet.WriteBytes(nbtFile.GetBuffer());
        }
    }
}
