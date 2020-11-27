using LiteNetwork.Protocol;
using Redstone.Protocol.Abstractions;
using System;

namespace Redstone.Protocol
{
    public class MinecraftPacket : LitePacket, IMinecraftPacket
    {
        public int PacketId { get; }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in write-only mode.
        /// </summary>
        public MinecraftPacket(int packetId)
        {
            PacketId = packetId;
            WriteVarInt32(packetId);
        }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in read-only mode.
        /// </summary>
        /// <param name="buffer">Packet buffer data.</param>
        public MinecraftPacket(int packetId, byte[] buffer) 
            : base(buffer)
        {
            PacketId = packetId;
        }

        public Guid ReadUUID()
        {
            throw new NotImplementedException();
        }

        public int ReadVarInt32()
        {
            int numRead = 0;
            int result = 0;
            byte read;

            do
            {
                read = ReadByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new InvalidOperationException("VarInt32 is too big.");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public long ReadVarInt64()
        {
            int numRead = 0;
            long result = 0;
            byte read;

            do
            {
                read = ReadByte();
                long value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 10)
                {
                    throw new InvalidOperationException("VarInt64 is too big.");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public void WriteUUID(Guid value)
        {
            throw new NotImplementedException();
        }

        public void WriteVarInt32(int value)
        {
            throw new NotImplementedException();
        }

        public void WriteVarInt64(long value)
        {
            throw new NotImplementedException();
        }
    }
}
