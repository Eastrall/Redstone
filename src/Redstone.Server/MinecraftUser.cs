using LiteNetwork.Server;
using System;

namespace Redstone.Server
{
    public class MinecraftUser : LiteServerUser
    {
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
