using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Configuration;
using Redstone.Common.IO;
using Redstone.Common.Structures.Biomes;
using Redstone.Common.Structures.Dimensions;
using Redstone.NBT;
using Redstone.NBT.Serialization;
using Redstone.NBT.Tags;
using Redstone.Protocol;
using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Protocol.Packets.Login;
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

            if (_serverConfiguration.Value.Mode == ServerModeType.Offline)
            {
                SendLoginSucess(user);
                user.Status = MinecraftUserStatus.Play;
                SendJoinGame(user);
                // TODO: held item changed
                // TODO: declare recipes
                // TODO: Tags
                // TODO: Entity status
                // TODO: declare commands
                // TODO: Unlock recipes
                // TODO: Player position and look
                SendPlayerInfo(user, PlayerInfoActionType.Add); 
                SendPlayerInfo(user, PlayerInfoActionType.UpdateLatency);
                SendUpdateViewPosition(user);
                // TODO: Update light
                SendChunkData(user);
                // TODO: World border
                SendSpawnPosition(user);
                SendPlayerPositionAndLook(user);

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

            joinPacket.WriteInt32(1); // EntityID
            joinPacket.WriteBoolean(_gameConfiguration.Value.IsHardcore); // Is hardcore
            joinPacket.WriteByte((byte)ServerGameModeType.Creative); // GameMode
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
            //IChunkSection chunkSection = chunk.GetSection(0);
            bool fullChunk = true;

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    IBlock block = _blockFactory.CreateBlock(BlockType.Grass);
                    chunk.SetBlock(block, x, 0, z);
                }
            }

            //chunk.SetBlock(_blockFactory.CreateBlock<GrassBlock>(), 0, 0, 0);
            //chunk.SetBlock(_blockFactory.CreateBlock<GrassBlock>(), 0, 0, 1);
            //chunk.SetBlock(_blockFactory.CreateBlock<GrassBlock>(), 0, 1, 1);
            chunk.GenerateHeightMap();

            using var packet = new ChunkDataPacket();

            packet.WriteInt32(chunk.X); // Chunk X
            packet.WriteInt32(chunk.Z); // Chunk Z
            packet.WriteBoolean(fullChunk); // full chunk

            int mask = 0;

            // if full chunk
            using var chunkStream = new MinecraftStream();
            for (int i = 0; i < chunk.Sections.Count(); i++)
            {
                IChunkSection section = chunk.Sections.ElementAt(i);

                if (fullChunk)
                {
                    mask |= 1 << i;
                    section.Serialize(chunkStream);
                }
            }

            packet.WriteVarInt32(mask);

            // Heightmap serialization
            //var heightmapCompound = new NbtCompound("")
            //{
            //    new NbtLongArray("MOTION_BLOCKING", chunk.Heightmap.ToArray())
            //};
            //var nbtFile = new NbtFile(heightmapCompound);

            var writer = new NbtWriter(packet, "");
            writer.WriteLongArray("MOTION_BLOCKING", chunk.Heightmap.ToArray());
            writer.WriteLongArray("WORLD_SURFACE", chunk.WorldSurfaceHeightmap.ToArray());
            writer.EndCompound();
            writer.Finish();

            //packet.WriteBytes(nbtFile.GetBuffer());

            // Biomes
            packet.WriteVarInt32(1024);
            for (int i = 0; i < 1024; i++)
            {
                packet.WriteVarInt32(4);
            }

            chunkStream.Position = 0;

            packet.WriteVarInt32((int)chunkStream.Length);
            packet.WriteBytes(chunkStream.Buffer);
            //packet.WriteBytes(chunkStream.GetBuffer());

            packet.WriteVarInt32(0); // block count
            // foreach block in blocks in chunk as NBT

            user.Send(packet);
        }

        private void SendSpawnPosition(MinecraftUser user)
        {
            using var packet = new SpawnPositionPacket(new Position(0, 1, 0));

            user.Send(packet);
        }

        private void SendPlayerPositionAndLook(MinecraftUser user)
        {
            using var packet = new PlayerPositionAndLookPacket();

            packet.WriteDouble(0); // x
            packet.WriteDouble(1); // y
            packet.WriteDouble(0); // z
            packet.WriteSingle(0); // yaw
            packet.WriteSingle(0); // pitch
            packet.WriteByte(0x01 | 0x02 | 0x04); // position flags (x|y|z)
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
