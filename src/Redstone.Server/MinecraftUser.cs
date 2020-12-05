using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Common;
using Redstone.Common.Codecs.Biomes;
using Redstone.Common.Codecs.Dimensions;
using Redstone.Common.Configuration;
using Redstone.Common.Serialization;
using Redstone.Common.Server;
using Redstone.NBT;
using Redstone.NBT.Tags;
using Redstone.Protocol;
using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Protocol.Packets.Handskake;
using Redstone.Protocol.Packets.Handskake.Server;
using Redstone.Protocol.Packets.Login;
using Redstone.Protocol.Packets.Status;
using Redstone.Protocol.Packets.Status.Client;
using Redstone.Protocol.Packets.Status.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Redstone.Server
{
    public class MinecraftUser : LiteServerUser
    {
        private readonly ILogger<MinecraftUser> _logger;
        private readonly IOptionsSnapshot<ServerConfiguration> _serverConfiguration;
        private readonly IRedstoneServer _server;

        public MinecraftUserStatus Status { get; private set; } = MinecraftUserStatus.Handshaking;

        public MinecraftUser(ILogger<MinecraftUser> logger, IOptionsSnapshot<ServerConfiguration> serverConfiguration)
        {
            _logger = logger;
            _serverConfiguration = serverConfiguration;
        }

        public override async Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            if (incomingPacketStream is not IMinecraftPacket packet)
            {
                throw new InvalidOperationException("Incoming packet is not a Minecraft packet.");
            }

            try
            {
                _logger.LogInformation($"Current minecraft client status: {Status} | Packet: 0x{packet.PacketId:X2}");

                switch (Status)
                {
                    case MinecraftUserStatus.Handshaking:
                        await OnHandshakingAsync(packet);
                        break;
                    case MinecraftUserStatus.Status:
                        await OnStatusAsync(packet);
                        break;
                    case MinecraftUserStatus.Login:
                        await OnLoginAsync(packet);
                        break;
                    case MinecraftUserStatus.Play:
                        await OnPlayAsync(packet);
                        break;
                    default: throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while handling a message during '{Status}' state.");
                throw;
            }
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"New client connected with id: '{Id}'.");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Client '{Id}' disconnected.");
        }

        private Task OnHandshakingAsync(IMinecraftPacket packet)
        {
            var packetId = (ServerHandshakePacketType)packet.PacketId;

            _logger.LogInformation($"Received Handshake packet: {packetId}");

            if (packetId == ServerHandshakePacketType.Handshaking)
            {
                var handshake = new HandshakePacket(packet);

                Status = handshake.NextState;
            }

            return Task.CompletedTask;
        }

        private Task OnStatusAsync(IMinecraftPacket packet)
        {
            var packetId = (ServerStatusPacketType)packet.PacketId;

            _logger.LogInformation($"Received Status packet: {packetId}");

            switch (packetId)
            {
                case ServerStatusPacketType.Request:
                    var serverInfo = new MinecraftServerStatus
                    {
                        Version = new MinecraftServerVersion { Name = "Redstone dev", Protocol = 754 },
                        Players = new MinecraftServerPlayers
                        {
                            Max = _serverConfiguration.Value.MaxPlayers,
                            Online = 0
                        },
                        Description = new MinecraftServerDescription
                        {
                            Text = _serverConfiguration.Value.Description
                        },
                        Favicon = RedstoneContants.GetDefaultFavicon()
                    };

                    using (var responsePacket = new StatusResponsePacket(serverInfo))
                    {
                        Send(responsePacket);
                    }
                    break;
                case ServerStatusPacketType.Ping:
                    var pingPacket = new StatusPingPacket(packet);

                    using (var pongPacket = new StatusPongPacket(pingPacket.Payload))
                    {
                        Send(pongPacket);
                    }
                    break;
            }

            return Task.CompletedTask;
        }

        private Task OnLoginAsync(IMinecraftPacket packet)
        {
            var packetId = (ServerLoginPacketType)packet.PacketId;

            _logger.LogInformation($"Received Login packet: {packetId}");

            switch (packetId)
            {
                case ServerLoginPacketType.LoginStart:
                    string username = packet.ReadString();

                    _logger.LogInformation($"{username} trying to log-in");

                    if (_serverConfiguration.Value.Mode == ServerModeType.Offline)
                    {
                        IEnumerable<Dimension> dimensions = LoadDimensions();
                        IEnumerable<Biome> biomes = LoadBiomes();

                        using var p = new MinecraftPacket(ClientLoginPacketType.LoginSuccess);
                        p.WriteUUID(Id);
                        p.WriteString(username);
                        Send(p);

                        Status = MinecraftUserStatus.Play;

                        using var joinPacket = new JoinGamePacket();
                        joinPacket.WriteInt32(1); // EntityID
                        joinPacket.WriteBoolean(false); // Is hardcore
                        joinPacket.WriteByte((byte)ServerGameModeType.Creative); // GameMode
                        joinPacket.WriteSByte((sbyte)ServerGameModeType.Unknown); // Previous game mode
                        joinPacket.WriteVarInt32(1); // World count
                        joinPacket.WriteString("overworld"); // Worlds name FOREACH()

                        // TODO: NBT for Dimensions and Biomes
                        WriteDimensionsAndBiomes(dimensions, biomes, joinPacket);

                        Dimension currentDimension = dimensions.First();
                        WriteDimension(currentDimension, joinPacket);
                        joinPacket.WriteString(currentDimension.Name); // World name identifier

                        joinPacket.WriteInt64(0); // Seed
                        joinPacket.WriteVarInt32((int)_serverConfiguration.Value.MaxPlayers); // Max players
                        joinPacket.WriteVarInt32(Math.Clamp(5, 2, 32)); // Render distance (2-32 chunks)
                        joinPacket.WriteBoolean(false); // Reduced debug info
                        joinPacket.WriteBoolean(true); // Respawn screen
                        joinPacket.WriteBoolean(true); // Is debug
                        joinPacket.WriteBoolean(true); // is flat terrain
                        Send(joinPacket);

                        break;
                    }

                    // TODO: login to Mojang API

                    break;
            }

            return Task.CompletedTask;
        }

        private Task OnPlayAsync(IMinecraftPacket packet)
        {
            //var packetId = (ServerLoginPacketType)packet.PacketId;

            _logger.LogInformation($"Received Login packet: 0x{packet.PacketId:X2}");
            return Task.CompletedTask;
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
            // Dimension serialization

            var nbtDimensionCompound = new NbtCompound("minecraft:dimension_type")
            {
                new NbtString("type", "minecraft:dimension_type")
            };

            var nbtDimensionList = new NbtList("value");
            foreach (Dimension dimension in dimensions)
            {
                var dimensionNbt = new NbtCompound()
                {
                    new NbtString("name", dimension.Name),
                    new NbtInt("id", dimension.Id),
                    new NbtCompound("element")
                    {
                        new NbtByte("piglin_safe", Convert.ToByte(dimension.Element.PiglinSafe)),
                        new NbtByte("natural", Convert.ToByte(dimension.Element.IsNatural)),
                        new NbtFloat("ambient_light", dimension.Element.AmbientLight),
                        new NbtString("infiniburn", dimension.Element.Infiniburn),
                        new NbtByte("respawn_anchor_works", Convert.ToByte(dimension.Element.RespawnAnchorWorks)),
                        new NbtByte("has_skylight", Convert.ToByte(dimension.Element.HasSkylight)),
                        new NbtByte("bed_works", Convert.ToByte(dimension.Element.BedWorks)),
                        new NbtString("effects", dimension.Element.Effects),
                        new NbtByte("has_raids", Convert.ToByte(dimension.Element.HasRaids)),
                        new NbtInt("logical_height", dimension.Element.LogicalHeight),
                        new NbtFloat("coordinate_scale", dimension.Element.CoordinateScale),
                        new NbtByte("ultrawarm", Convert.ToByte(dimension.Element.IsUltrawarm)),
                        new NbtByte("has_ceiling", Convert.ToByte(dimension.Element.HasCeiling))
                    }
                };

                nbtDimensionList.Add(dimensionNbt);
            }

            nbtDimensionCompound.Add(nbtDimensionList);

            // Biomes serialization

            var nbtBiomes = new NbtCompound("minecraft:worldgen/biome")
            {
                new NbtString("type", "minecraft:worldgen/biome")
            };
            var nbtBiomeList = new NbtList("value");

            foreach (Biome biome in biomes)
            {
                var nbtBiomeCompound = new NbtCompound()
                {
                    new NbtString("name", biome.Name),
                    new NbtInt("id", biome.Id),
                    new NbtCompound("element")
                    {
                        new NbtFloat("precipitation", (float)biome.Element.Precipitation),
                        new NbtFloat("depth", biome.Element.Depth),
                        new NbtFloat("temperature", biome.Element.Temperature),
                        new NbtFloat("scale", biome.Element.Scale),
                        new NbtFloat("downfall", biome.Element.DownFall),
                        new NbtFloat("category", (float)biome.Element.Category),
                        new NbtCompound("effects")
                        {
                            new NbtFloat("sky_color", biome.Element.Effects.SkyColor),
                            new NbtFloat("water_fog_color", biome.Element.Effects.WaterFogColor),
                            new NbtFloat("fog_color", biome.Element.Effects.FogColor),
                            new NbtFloat("water_color", biome.Element.Effects.WaterColor),
                            new NbtCompound("mood_sound")
                            {
                                new NbtInt("tick_delay", biome.Element.Effects.MoodSound.TickDelay),
                                new NbtDouble("offset", biome.Element.Effects.MoodSound.Offset),
                                new NbtString("sound", biome.Element.Effects.MoodSound.Sound),
                                new NbtInt("block_search_extent", biome.Element.Effects.MoodSound.BlockSearchExtent)
                            }
                        }
                    }
                };

                nbtBiomeList.Add(nbtBiomeCompound);
            }

            nbtBiomes.Add(nbtBiomeList);

            var nbtCompound = new NbtCompound("")
            {
                nbtDimensionCompound,
                nbtBiomes
            };
            var nbtFile = new NbtFile(nbtCompound);

            using var memoryStream = new MemoryStream();
            nbtFile.SaveToStream(memoryStream, NbtCompression.None);

            byte[] nbtBuffer = memoryStream.GetBuffer();

            packet.WriteBytes(nbtBuffer);
        }

        private void WriteDimension(Dimension dimension, IMinecraftPacket packet)
        {

        }
    }
}
