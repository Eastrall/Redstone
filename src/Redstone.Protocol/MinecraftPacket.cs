using LiteNetwork.Protocol;
using Redstone.Protocol.Abstractions;
using System;

namespace Redstone.Protocol
{
    public class MinecraftPacket : LitePacketStream, IMinecraftPacket
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
            var guidData = new byte[sizeof(ulong) * 2];
            ulong a = ReadUInt64();
            ulong b = ReadUInt64();

            Array.Copy(BitConverter.GetBytes(a), guidData, sizeof(ulong));
            Array.Copy(BitConverter.GetBytes(b), 0, guidData, sizeof(ulong), sizeof(ulong));

            return new Guid(guidData);
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
            var bytes = value.ToByteArray();
            var long1 = BitConverter.ToUInt64(bytes, 0);
            var long2 = BitConverter.ToUInt64(bytes, 8);

            WriteUInt64(long1);
            WriteUInt64(long2);
        }

        public void WriteVarInt32(int value)
        {
            var valueToWrite = (uint)value;

            do
            {
                var temp = (byte)(valueToWrite & 127);

                valueToWrite >>= 7;

                if (valueToWrite != 0)
                {
                    temp |= sbyte.MaxValue + 1;
                }

                WriteByte(temp);
            } while (valueToWrite != 0);
        }

        public void WriteVarInt64(long value)
        {
            var valueToWrite = (ulong)value;

            do
            {
                var temp = (byte)(valueToWrite & 127);

                valueToWrite >>= 7;

                if (valueToWrite != 0)
                {
                    temp |= sbyte.MaxValue + 1;
                }

                WriteByte(temp);
            } while (valueToWrite != 0);
        }
    }
}
