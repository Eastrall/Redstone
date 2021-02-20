using System;

namespace Redstone.Common.IO
{
    /// <summary>
    /// Provides a mechanism to build a Minecraft data stream.
    /// </summary>
    public class MinecraftStream : BinaryStream
    {
        public MinecraftStream()
        {
        }

        public MinecraftStream(byte[] buffer)
            : base(buffer)
        {
        }

        public MinecraftStream(int capacity)
            : base(capacity)
        {
        }

        public MinecraftStream(byte[] buffer, bool writable)
            : base(buffer, writable)
        {
        }

        public MinecraftStream(byte[] buffer, int index, int count)
            : base(buffer, index, count)
        {
        }

        public MinecraftStream(byte[] buffer, int index, int count, bool writable)
            : base(buffer, index, count, writable)
        {
        }

        public MinecraftStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
            : base(buffer, index, count, writable, publiclyVisible)
        {
        }

        /// <summary>
        /// Reads an Univeral Unique IDentifier value from the packet stream.
        /// </summary>
        /// <returns>UUID</returns>
        /// <remarks>
        /// A UUID is a 128 bit signed integer.
        /// </remarks>
        public Guid ReadUUID()
        {
            var guidData = new byte[sizeof(ulong) * 2];
            ulong a = ReadUInt64();
            ulong b = ReadUInt64();

            Array.Copy(BitConverter.GetBytes(a), guidData, sizeof(ulong));
            Array.Copy(BitConverter.GetBytes(b), 0, guidData, sizeof(ulong), sizeof(ulong));

            return new Guid(guidData);
        }

        /// <summary>
        /// Reads a 4 byte variable integer value from the packet stream.
        /// </summary>
        /// <returns>Integer value.</returns>
        public int ReadVarInt32()
        {
            int numRead = 0;
            int result = 0;
            byte read;

            do
            {
                read = (byte)ReadByte();
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

        /// <summary>
        /// Reads a 8 bytes variable numeric value from the packet stream.
        /// </summary>
        /// <returns>Long numeric value.</returns>
        public long ReadVarInt64()
        {
            int numRead = 0;
            long result = 0;
            byte read;

            do
            {
                read = (byte)ReadByte();
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

        /// <summary>
        /// Writes an Universal Unique IDentifier into the packet stream.
        /// </summary>
        /// <param name="value"></param>
        public void WriteUUID(Guid value)
        {
            var bytes = value.ToByteArray();
            var long1 = BitConverter.ToUInt64(bytes, 0);
            var long2 = BitConverter.ToUInt64(bytes, 8);

            WriteUInt64(long1);
            WriteUInt64(long2);
        }

        /// <summary>
        /// Writes a 4 byte variable numeric value into the packet stream.
        /// </summary>
        /// <param name="value">Numeric integer value.</param>
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

        /// <summary>
        /// Writes a 8 byte variable numeric value into the packet stream.
        /// </summary>
        /// <param name="value">Numeric long value.</param>
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
