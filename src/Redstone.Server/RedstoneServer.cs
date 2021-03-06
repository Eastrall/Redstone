﻿using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
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
        private readonly IRegistry _registry;

        public RSAParameters ServerEncryptionKey { get; private set; }

        public IEnumerable<MinecraftUser> ConnectedPlayers => ConnectedUsers;

        public uint ConnectedPlayersCount => (uint)ConnectedUsers.Count(x => x.Status == MinecraftUserStatus.Play);

        public RedstoneServer(LiteServerConfiguration configuration, ILitePacketProcessor packetProcessor = null, IServiceProvider serviceProvider = null) 
            : base(configuration, packetProcessor, serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<RedstoneServer>>();
            _packetEncryption = serviceProvider.GetRequiredService<IMinecraftPacketEncryption>();
            _serverConfiguration = serviceProvider.GetRequiredService<IOptions<ServerConfiguration>>();
            _registry = serviceProvider.GetRequiredService<IRegistry>();
        }

        protected override void OnBeforeStart()
        {
            _registry.Load();

            _logger.LogInformation("Generating server encryption keys...");
            ServerEncryptionKey = _packetEncryption.GenerateEncryptionKeys();
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"Server started and listening on port '{Configuration.Port}'.");
        }

        public MinecraftServerStatus GetServerStatus()
        {
            return new MinecraftServerStatus
            {
                Version = new MinecraftServerVersion { Name = "Redstone dev", Protocol = 754 },
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
    }
}
