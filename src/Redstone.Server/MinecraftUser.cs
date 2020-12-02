using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Common;
using Redstone.Common.Server;
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
                _logger.LogInformation($"Current minecraft client status: {Status}");

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
                        joinPacket.WriteString("THE WORLD"); // Worlds name FOREACH()
                        // TODO: NBT for Dimensions
                        // TODO: NBT for biomes
                        joinPacket.WriteString("minecraft:world"); // World name identifier
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
    }
}
