using LiteNetwork.Protocol.Abstractions;
using LiteNetwork.Server;
using Redstone.Protocol.Abstractions;
using System;
using System.Threading.Tasks;

namespace Redstone.Server
{
    public class MinecraftUser : LiteServerUser
    {
        public override Task HandleMessageAsync(ILitePacketStream incomingPacketStream)
        {
            if (incomingPacketStream is not IMinecraftPacket packet)
            {
                return Task.CompletedTask;
            }

            int protocolVersion = packet.ReadVarInt32();

            Console.WriteLine($"Protocol version: {protocolVersion}");
            
            return base.HandleMessageAsync(incomingPacketStream);
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"New client connected with id: '{Id}'.");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Client '{Id}' disconnected.");
        }
    }
}
