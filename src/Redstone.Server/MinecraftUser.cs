using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
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
        private readonly IOptionsSnapshot<GameConfiguration> _gameConfiguration;
        private readonly IRedstoneServer _server;
        private readonly IPacketHandler _packetHandler;

        public MinecraftUserStatus Status { get; private set; } = MinecraftUserStatus.Handshaking;

        public MinecraftUser(ILogger<MinecraftUser> logger, IOptionsSnapshot<ServerConfiguration> serverConfiguration, IOptionsSnapshot<GameConfiguration> gameConfiguration, IRedstoneServer server, IPacketHandler packetHandler)
        {
            _logger = logger;
            _serverConfiguration = serverConfiguration;
            _gameConfiguration = gameConfiguration;
            _server = server;
            _packetHandler = packetHandler;
        }

        public override Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            if (incomingPacketStream is not IMinecraftPacket packet)
            {
                throw new InvalidOperationException("Incoming packet is not a Minecraft packet.");
            }

            try
            {
                _logger.LogInformation($"Current minecraft client status: {Status} | Packet: 0x{packet.PacketId:X2}");
                _packetHandler.Invoke(this, Status, packet.PacketId);

                //switch (Status)
                //{
                //    case MinecraftUserStatus.Handshaking:
                //        await OnHandshakingAsync(packet);
                //        break;
                //    case MinecraftUserStatus.Status:
                //        await OnStatusAsync(packet);
                //        break;
                //    case MinecraftUserStatus.Login:
                //        await OnLoginAsync(packet);
                //        break;
                //    case MinecraftUserStatus.Play:
                //        await OnPlayAsync(packet);
                //        break;
                //    default: throw new NotImplementedException();
                //}
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while handling a message during '{Status}' state.");
                throw;
            }

            return Task.CompletedTask;
        }

        protected override void OnConnected()
        {
            _logger.LogInformation($"New client connected with id: '{Id}'.");
        }

        protected override void OnDisconnected()
        {
            _logger.LogInformation($"Client '{Id}' disconnected.");
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
                    using (var responsePacket = new StatusResponsePacket(_server.GetServerStatus()))
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

            _logger.LogInformation($"Received Play packet: 0x{packet.PacketId:X2}");
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
