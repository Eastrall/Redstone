using LiteNetwork.Protocol.Abstractions;
using System;

namespace Redstone.Protocol
{
    public class MinecraftPacketProcessor : ILitePacketProcessor
    {
        public int HeaderSize => sizeof(int) * 2;

        public bool IncludeHeader => false;

        public ILitePacketStream CreatePacket(byte[] buffer)
        {
            return new MinecraftPacket(0, buffer);
        }

        public int GetMessageLength(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
