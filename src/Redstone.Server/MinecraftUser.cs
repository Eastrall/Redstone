using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Common;
using Redstone.Common.Server;
using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Packets.Handskake;
using Redstone.Protocol.Packets.Handskake.Serverbound;
using Redstone.Protocol.Packets.Status;
using Redstone.Protocol.Packets.Status.Clientbound;
using Redstone.Protocol.Packets.Status.Serverbound;
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

        public override Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            if (incomingPacketStream is not IMinecraftPacket packet)
            {
                throw new InvalidOperationException("Incoming packet is not a Minecraft packet.");
            }

            try
            {
                Task task = Status switch
                {
                    MinecraftUserStatus.Handshaking => OnHandshakingAsync(packet),
                    MinecraftUserStatus.Status => OnStatusAsync(packet),
                    MinecraftUserStatus.Login => OnLoginAsync(packet),
                    MinecraftUserStatus.Play => OnPlayerAsync(packet),
                    _ => throw new NotImplementedException(),
                };

                return task;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while handling a message during '{Status}' state.");

                return Task.CompletedTask;
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
            if (packet.PacketId == (int)HandshakeServerBoundPacketType.Handshaking)
            {
                var handshake = new HandshakePacket(packet);

                Status = handshake.NextState;

                Console.WriteLine($"Protocol version: {handshake.ProtocolVersion}");
                Console.WriteLine($"Host: {handshake.ServerAddress}");
                Console.WriteLine($"Port: {handshake.ServerPort}");
                Console.WriteLine($"Next State: {handshake.NextState}");
            }

            return Task.CompletedTask;
        }

        private Task OnStatusAsync(IMinecraftPacket packet)
        {
            var packetId = (StatusServerboundPacketType)packet.PacketId;

            switch (packetId)
            {
                case StatusServerboundPacketType.Request:
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
                case StatusServerboundPacketType.Ping:
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
            return Task.CompletedTask;
        }

        private Task OnPlayerAsync(IMinecraftPacket packet)
        {
            return Task.CompletedTask;
        }
    }
}
