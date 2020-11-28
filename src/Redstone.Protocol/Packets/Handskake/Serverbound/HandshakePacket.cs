using Redstone.Common;
using Redstone.Protocol.Abstractions;

namespace Redstone.Protocol.Packets.Handskake.Serverbound
{
    public class HandshakePacket
    {
        public int ProtocolVersion { get; private set; }

        public string ServerAddress { get; private set; }

        public ushort ServerPort { get; private set; }

        public MinecraftUserStatus NextState { get; private set; }

        public HandshakePacket(IMinecraftPacket packet)
        {
            ProtocolVersion = packet.ReadVarInt32();
            ServerAddress = packet.ReadString();
            ServerPort = packet.ReadUInt16();
            NextState = (MinecraftUserStatus)packet.ReadVarInt32();
        }
    }
}
