using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Abstractions;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common.Configuration;
using Redstone.Common.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Redstone.Server
{
    public class RedstoneServer : LiteServer<MinecraftUser>, IRedstoneServer
    {
        private readonly ILogger<RedstoneServer> _logger;
        private readonly IMinecraftPacketEncryption _packetEncryption;
        private readonly IOptions<ServerConfiguration> _serverConfiguration;
        private readonly IOptions<GameConfiguration> _gameConfiguration;
        private readonly IRegistry _registry;
        private readonly IWorld _worldManager;

        public IRedstoneServerEvents Events { get; }

        public RSAParameters ServerEncryptionKey { get; private set; }

        public IEnumerable<IMinecraftUser> ConnectedPlayers => ConnectedUsers.Where(x => x.Status == MinecraftUserStatus.Play);

        public uint ConnectedPlayersCount => (uint)ConnectedUsers.Count(x => x.Status == MinecraftUserStatus.Play);

        public RedstoneServer(LiteServerConfiguration configuration, ILitePacketProcessor packetProcessor = null, IServiceProvider serviceProvider = null) 
            : base(configuration, packetProcessor, serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<RedstoneServer>>();
            _packetEncryption = serviceProvider.GetRequiredService<IMinecraftPacketEncryption>();
            _serverConfiguration = serviceProvider.GetRequiredService<IOptions<ServerConfiguration>>();
            _gameConfiguration = serviceProvider.GetRequiredService<IOptions<GameConfiguration>>();
            _registry = serviceProvider.GetRequiredService<IRegistry>();
            _worldManager = serviceProvider.GetRequiredService<IWorld>();
            Events = new RedstoneServerEvents(this);
        }

        protected override void OnBeforeStart()
        {
            _registry.Load();
            _worldManager.Load(_gameConfiguration.Value.LevelName);

            _logger.LogInformation("Generating server encryption keys...");
            ServerEncryptionKey = _packetEncryption.GenerateEncryptionKeys();
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"Server started and listening on port '{Configuration.Port}'.");
        }

        public void SendTo(IEnumerable<IMinecraftUser> users, IMinecraftPacket packet) 
            => base.SendTo(users.Cast<MinecraftUser>(), packet);

        public MinecraftServerStatus GetServerStatus()
        {
            return new MinecraftServerStatus
            {
                Version = new MinecraftServerVersion
                {
                    Name = _serverConfiguration.Value.Name,
                    Protocol = _serverConfiguration.Value.ProtocolVersion
                },
                Players = new MinecraftServerPlayers
                {
                    Max = _serverConfiguration.Value.MaxPlayers,
                    Online = ConnectedPlayersCount
                },
                Description = new MinecraftServerDescription
                {
                    Text = _serverConfiguration.Value.Description
                },
                Favicon = RedstoneContants.GetDefaultFavicon()
            };
        }

        public IMinecraftUser GetUser(string username) => ConnectedUsers.FirstOrDefault(x => x.Username == username);

        public bool HasUser(string username) => ConnectedUsers.Any(x => x.Username == username);
    }
}
