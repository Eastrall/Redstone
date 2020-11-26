using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using System;

namespace Redstone.Server
{
    public class RedstoneServer : LiteServer<MinecraftUser>
    {
        public RedstoneServer(LiteServerConfiguration configuration, ILitePacketProcessor packetProcessor = null, IServiceProvider serviceProvider = null) 
            : base(configuration, packetProcessor, serviceProvider)
        {
        }

        protected override void OnAfterStart()
        {
            Console.WriteLine($"Server started and listening on port '{Configuration.Port}'.");
        }
    }
}
