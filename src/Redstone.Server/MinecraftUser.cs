﻿using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Handlers.Exceptions;
using Redstone.Protocol.Packets.Game;
using Redstone.Protocol.Packets.Handskake;
using Redstone.Protocol.Packets.Login;
using Redstone.Protocol.Packets.Status;
using Redstone.Server.Entities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Redstone.Server
{
    [DebuggerDisplay("{Username ?? \"[undefined]\"}: {Status}")]
    public class MinecraftUser : LiteServerUser, IMinecraftUser
    {
        private readonly ILogger<MinecraftUser> _logger;
        private readonly IPacketHandler _packetHandler;
        private readonly IPlayer _player;

        public MinecraftUserStatus Status { get; internal set; } = MinecraftUserStatus.Handshaking;

        public string Username { get; internal set; }

        public IPlayer Player => _player;

        public MinecraftUser(ILogger<MinecraftUser> logger, IPacketHandler packetHandler)
        {
            _logger = logger;
            _packetHandler = packetHandler;
            _player = new Player(this);
        }

        public void Send(IMinecraftPacket packet) => base.Send(packet);

        public void Disconnect() => Disconnect(null);

        public void Disconnect(string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason))
            {
                _logger.LogInformation($"{Username} disconnected. Reason: {reason}");
            }

            Socket.Close();
        }

        public override Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            if (incomingPacketStream is not IMinecraftPacket packet)
            {
                throw new InvalidOperationException("Incoming packet is not a Minecraft packet.");
            }

            object packetHeader = GetMinecraftPacketType(packet.PacketId);

            try
            {
                _logger.LogTrace($"[{Username}] [{Status}] | Packet: {packetHeader} (0x{packet.PacketId:X2})");
                _packetHandler.Invoke(Status, packetHeader, this, packet);
            }
            catch (HandlerActionNotFoundException)
            {
                if (!Socket.Connected)
                {
                    return Task.CompletedTask;
                }

                if (Enum.IsDefined(packetHeader.GetType(), packet.PacketId))
                {
                    _logger.LogTrace($"[{Username}] Received an unimplemented {Status} packet {packetHeader} (0x{packet.PacketId:X2}) from {Socket.RemoteEndPoint}");
                }
                else
                {
                    _logger.LogTrace($"[{Username}] Received an unknown {Status} packet: 0x{packet.PacketId:X2} from {Socket.RemoteEndPoint}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while handling packet '{packetHeader}' during '{Status}' state.");

                if (Status != MinecraftUserStatus.Play)
                {
                    Socket.Close();
                }

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

            if (Status == MinecraftUserStatus.Play)
            {
                Player.Map.RemovePlayer(Player);
                // TODO: save current player
            }
        }

        private object GetMinecraftPacketType(int packetId)
        {
            Type type = Status switch
            {
                MinecraftUserStatus.Handshaking => typeof(ServerHandshakePacketType),
                MinecraftUserStatus.Status => typeof(ServerStatusPacketType),
                MinecraftUserStatus.Login => typeof(ServerLoginPacketType),
                MinecraftUserStatus.Play => typeof(ServerPlayPacketType),
                _ => throw new NotImplementedException()
            };

            return Enum.ToObject(type, packetId);
        }
    }
}
