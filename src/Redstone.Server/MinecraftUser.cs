using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Common;
using Redstone.Common.Server;
using Redstone.Protocol;
using Redstone.Protocol.Abstractions;
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
                        // TODO: send login success
                        break;
                    }

                    // TODO: login to Mojang API

                    break;
            }

            return Task.CompletedTask;
        }

        private Task OnPlayAsync(IMinecraftPacket packet)
        {
            return Task.CompletedTask;
        }
    }
}
