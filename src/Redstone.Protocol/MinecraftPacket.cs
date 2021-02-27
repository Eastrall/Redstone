using LiteNetwork.Protocol;
using Redstone.Common;
using Redstone.Common.IO;
using Redstone.Protocol.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Redstone.Protocol
{
    /// <summary>
    /// Minecraft packet implementation.
    /// </summary>
    public class MinecraftPacket : LitePacketStream, IMinecraftPacket
    {
        public byte[] BaseBuffer => base.Buffer;

        public int PacketId { get; }

        /// <summary>
        /// Gets the final <see cref="MinecraftPacket"/> buffer including the packet size.
        /// </summary>
        public override byte[] Buffer
        {
            get
            {
                using var stream = new MinecraftStream();

                stream.WriteVarInt32(GetVarInt32Length(PacketId) + (int)Length);
                stream.WriteVarInt32(PacketId);
                stream.WriteBytes(BaseBuffer);

                return stream.Buffer;
            }
        }

        /// <summary>
        /// Creates a new empty <see cref="MinecraftPacket"/> stream in write-only mode.
        /// </summary>
        public MinecraftPacket()
        {
        }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in write-only mode with a given packet id as an integer.
        /// </summary>
        /// <param name="packetId">Packet Id.</param>
        public MinecraftPacket(int packetId)
        {
            PacketId = packetId;
        }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in write-only mode with a given packet id as an enum value.
        /// </summary>
        /// <param name="packetId">Packet Id.</param>
        public MinecraftPacket(Enum packetId)
            : this(Convert.ToInt32(packetId))
        {
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

        public override void WriteInt16(short value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteUInt16(ushort value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteInt32(int value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteUInt32(uint value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteInt64(long value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteUInt64(ulong value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteSingle(float value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteDouble(double value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public override void WriteBytes(byte[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values), "Failed to write a null byte array into the packet stream.");
            }

            Write(values, 0, values.Length);
        }

        public override string ReadString()
        {
            int stringLength = ReadVarInt32();
            byte[] stringBytes = ReadBytes(stringLength);

            return ReadEncoding.GetString(stringBytes);
        }

        public override void WriteString(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "Failed to write a null string into the packet stream.");
            }

            byte[] stringData = WriteEncoding.GetBytes(value);

            WriteVarInt32(stringData.Length);
            Write(stringData, 0, stringData.Length);
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
            var unsigned = (uint)value;

            do
            {
                var temp = (byte)(unsigned & 127);

                unsigned >>= 7;

                if (unsigned != 0)
                {
                    temp |= 128;
                }

                WriteByte(temp);
            } while (unsigned != 0);
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
                    temp |= 128;
                }

                WriteByte(temp);
            } while (valueToWrite != 0);
        }

        public Position ReadPosition()
        {
            ulong positionAsLong = ReadUInt64();
            var position = new Position
            {
                X = positionAsLong >> 38,
                Y = positionAsLong & 0xFFF,
                Z = positionAsLong << 26 >> 38
            };

            return position;
        }

        public void WritePosition(Position position)
        {
            var x = (((int)position.X & 0x3FFFFFF) << 38);
            var z = (((int)position.Z & 0x3FFFFFF) << 12);
            var y = ((int)position.Y & 0xFFF);

            WriteInt64(x | z | y);
        }

        public void Dump(string fileName, PacketDumpMode dumpMode)
        {
            var dumpContent = dumpMode switch
            {
                PacketDumpMode.Default => string.Join(Environment.NewLine, BaseBuffer),
                PacketDumpMode.UTF8String => Encoding.UTF8.GetString(BaseBuffer),
                _ => null,
            };

            if (!string.IsNullOrEmpty(dumpContent))
            {
                File.WriteAllText(fileName, dumpContent);
            }
        }

        private void InternalWriteBytes(byte[] values)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(values);
            }

            WriteBytes(values);
        }

        private static byte[] ConvertInt32ToVarInt32Buffer(int value)
        {
            var buffer = new List<byte>();
            var valueToWrite = (uint)value;

            do
            {
                var temp = (byte)(valueToWrite & 127);

                valueToWrite >>= 7;

                if (valueToWrite != 0)
                {
                    temp |= 128;
                }

                buffer.Add(temp);
            } while (valueToWrite != 0);

            return buffer.ToArray();
        }

        private static int GetVarInt32Length(int value)
        {
            int amount = 0;
            
            do
            {
                value >>= 7;
                amount++;
            } while (value != 0);

            return amount;
        }
    }
}
