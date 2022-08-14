using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Abstractions;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Events;
using Redstone.Abstractions.Protocol;
using Redstone.Common.Configuration;
using Redstone.Common.IO;
using Redstone.Common.Utilities;
using Redstone.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Handlers.Exceptions;
using Redstone.Protocol.Packets.Game;
using Redstone.Protocol.Packets.Handskake;
using Redstone.Protocol.Packets.Login;
using Redstone.Protocol.Packets.Status;
using Redstone.Server.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Redstone.Server
{
    [DebuggerDisplay("{Username ?? \"[undefined]\"}: {Status}")]
    public class MinecraftUser : LiteServerUser, IMinecraftUser
    {
        private readonly ILogger<MinecraftUser> _logger;
        private readonly IOptions<GameOptions> _gameConfiguration;
        private readonly IRedstoneServer _server;
        private readonly IPacketHandler _packetHandler;
        private readonly IServiceProvider _serviceProvider;
        private IPlayer _player;

        public MinecraftUserStatus Status { get; internal set; } = MinecraftUserStatus.Handshaking;

        public string Username { get; internal set; }

        public IPlayer Player => _player;

        public MinecraftUser(ILogger<MinecraftUser> logger, IOptions<GameOptions> gameConfiguration, IRedstoneServer server, IPacketHandler packetHandler, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _gameConfiguration = gameConfiguration;
            _server = server;
            _packetHandler = packetHandler;
            _serviceProvider = serviceProvider;
        }

        public void UpdateStatus(MinecraftUserStatus newStatus)
        {
            // TODO: do additionnal checks.
            Status = newStatus;
        }

        public void Send(IMinecraftPacket packet)
        {
            if (packet is MinecraftPacket minecraftPacket)
            {
                using var stream = new MinecraftStream();

                stream.WriteVarInt32(minecraftPacket.PacketLength);
                stream.WriteVarInt32(minecraftPacket.PacketId);
                stream.WriteBytes(minecraftPacket.Buffer);

                base.Send(stream.Buffer);
            }
        }

        public void Disconnect() => Disconnect(null);

        public void Disconnect(string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason))
            {
                _logger.LogInformation($"{Username} disconnected. Reason: {reason}");
            }

            _server.DisconnectUser(Id);
        }

        public void LoadPlayer(Guid playerId, string playerName)
        {
            if (_player is not null)
            {
                _logger.LogWarning($"Player is already loaded.");
                return;
            }

            Username = playerName;
            _player = new Player(this, playerId, playerName, _serviceProvider)
            {
                GameMode = _gameConfiguration.Value.Mode
            };

            // TODO: load player information from storage
        }

        public override Task HandleMessageAsync(byte[] incomingMessage)
        {
            int packetId = VariableStorageUtilities.ReadVarInt32(incomingMessage);
            int packetIdLength = VariableStorageUtilities.GetVarInt32Length(packetId);
            MinecraftPacket packet = new(packetId, incomingMessage.Skip(packetIdLength).ToArray());
            object packetHeader = GetMinecraftPacketType(packet.PacketId);

            try
            {
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
            _logger.LogInformation($"Client '{Id}' disconnected (Username: '{Username}').");

            if (Status == MinecraftUserStatus.Play && Player.Map is not null)
            {
                Player.Map.RemovePlayer(Player);
                // TODO: save current player

                _server.Events.OnPlayerLeaveGame(new PlayerLeaveEventArgs(Player));
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
