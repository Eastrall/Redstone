using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Common;
using Redstone.Common.Codecs.Biomes;
using Redstone.Common.Codecs.Dimensions;
using Redstone.Common.Configuration;
using Redstone.Common.Serialization;
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
using System.IO;
using System.Linq;

namespace Redstone.Server.Handlers.Login
{
    public class LoginHandler
    {
        private readonly ILogger<LoginHandler> _logger;
        private readonly IOptions<ServerConfiguration> _serverConfiguration;
        private readonly IOptions<GameConfiguration> _gameConfiguration;

        public LoginHandler(ILogger<LoginHandler> logger, IOptions<ServerConfiguration> serverConfiguration, IOptions<GameConfiguration> gameConfiguration)
        {
            _logger = logger;
            _serverConfiguration = serverConfiguration;
            _gameConfiguration = gameConfiguration;
        }

        [LoginPacketHandler(ServerLoginPacketType.LoginStart)]
        public void OnLogin(MinecraftUser user, IMinecraftPacket packet)
        {
            string username = packet.ReadString();

            _logger.LogInformation($"{username} trying to log-in");

            if (_serverConfiguration.Value.Mode == ServerModeType.Offline)
            {
                IEnumerable<Dimension> dimensions = LoadDimensions();
                IEnumerable<Biome> biomes = LoadBiomes();

                using var p = new MinecraftPacket(ClientLoginPacketType.LoginSuccess);
                p.WriteUUID(user.Id);
                p.WriteString(username);
                user.Send(p);

                user.Status = MinecraftUserStatus.Play;

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

                WriteDimensionsAndBiomes(dimensions, biomes, joinPacket);

                Dimension currentDimension = dimensions.First();
                WriteDimension(currentDimension, joinPacket);
                joinPacket.WriteString(currentDimension.Name); // World name identifier

                joinPacket.WriteInt64(_gameConfiguration.Value.Seed); // Seed
                joinPacket.WriteVarInt32((int)_serverConfiguration.Value.MaxPlayers); // Max players
                                                                                      // TODO: define constants for rendering distances
                joinPacket.WriteVarInt32(Math.Clamp(_gameConfiguration.Value.RenderingDistance, 2, 32)); // Render distance (2-32 chunks)
                joinPacket.WriteBoolean(_serverConfiguration.Value.ReducedDebugInfo); // Reduced debug info
                joinPacket.WriteBoolean(_gameConfiguration.Value.DisplayRespawnScreen); // Respawn screen
                joinPacket.WriteBoolean(_serverConfiguration.Value.Debug); // Is debug
                joinPacket.WriteBoolean(_serverConfiguration.Value.FlatTerrain); // is flat terrain
                user.Send(joinPacket);
            }

            // TODO: login to Mojang API
        }

        private IEnumerable<Dimension> LoadDimensions()
        {
            return JsonSerializer.Deserialize<IEnumerable<Dimension>>(File.ReadAllText("/opt/redstone/data/dimensions.json"));
        }
        private IEnumerable<Biome> LoadBiomes()
        {
            return JsonSerializer.Deserialize<IEnumerable<Biome>>(File.ReadAllText("/opt/redstone/data/biomes.json"));
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
