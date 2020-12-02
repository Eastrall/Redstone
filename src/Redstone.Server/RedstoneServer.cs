using LiteNetwork.Common;
using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Redstone.Protocol.Abstractions;
using System;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace Redstone.Server
{
    public class RedstoneServer : LiteServer<MinecraftUser>, IRedstoneServer
    {
        private readonly IMinecraftPacketEncryption _packetEncryption;

        public RSAParameters ServerEncryptionKey { get; private set; }

        public RedstoneServer(LiteServerConfiguration configuration, ILitePacketProcessor packetProcessor = null, IServiceProvider serviceProvider = null) 
            : base(configuration, packetProcessor, serviceProvider)
        {
            Configuration.ReceiveStrategy = ReceiveStrategyType.Queued;
            _packetEncryption = serviceProvider.GetRequiredService<IMinecraftPacketEncryption>();
        }

        protected override void OnBeforeStart()
        {
            ServerEncryptionKey = _packetEncryption.GenerateEncryptionKeys();
        }

        protected override void OnAfterStart()
        {
            Console.WriteLine($"Server started and listening on port '{Configuration.Port}'.");
        }
    }
}
