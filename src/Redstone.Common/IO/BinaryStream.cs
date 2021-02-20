using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Redstone.Common.IO
{
    public class BinaryStream : MemoryStream
    {
        public virtual byte[] Buffer => GetBuffer().Take((int)Length).ToArray();

        public BinaryStream()
        {
        }

        public BinaryStream(byte[] buffer) 
            : base(buffer)
        {
        }

        public BinaryStream(int capacity) 
            : base(capacity)
        {
        }

        public BinaryStream(byte[] buffer, bool writable) 
            : base(buffer, writable)
        {
        }

        public BinaryStream(byte[] buffer, int index, int count) 
            : base(buffer, index, count)
        {
        }

        public BinaryStream(byte[] buffer, int index, int count, bool writable) 
            : base(buffer, index, count, writable)
        {
        }

        public BinaryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible) 
            : base(buffer, index, count, writable, publiclyVisible)
        {
        }

        public void WriteSByte(sbyte value) => Write(BitConverter.GetBytes(value), 0, sizeof(sbyte));

        public void WriteChar(char value) => Write(BitConverter.GetBytes(value), 0, sizeof(char));

        public void WriteBoolean(bool value) => Write(BitConverter.GetBytes(value), 0, sizeof(bool));

        public void WriteInt16(short value) => Write(BitConverter.GetBytes(value), 0, sizeof(short));

        public void WriteUInt16(ushort value) => Write(BitConverter.GetBytes(value), 0, sizeof(ushort));

        public void WriteInt32(int value) => Write(BitConverter.GetBytes(value), 0, sizeof(int));

        public void WriteUInt32(uint value) => Write(BitConverter.GetBytes(value), 0, sizeof(uint));

        public void WriteInt64(long value) => Write(BitConverter.GetBytes(value), 0, sizeof(long));

        public void WriteUInt64(ulong value) => Write(BitConverter.GetBytes(value), 0, sizeof(ulong));

        public void WriteSingle(float value) => Write(BitConverter.GetBytes(value), 0, sizeof(float));

        public void WriteDouble(double value) => Write(BitConverter.GetBytes(value), 0, sizeof(double));

        public void WriteBytes(byte[] values) => Write(values, 0, values.Length);

        public void WriteString(string value)
        {
            WriteInt32(value.Length);
            WriteBytes(Encoding.UTF8.GetBytes(value));
        }

        public sbyte ReadSByte() => Read<sbyte>();

        public char ReadChar() => Read<char>();

        public bool ReadBoolean() => Read<bool>();

        public short ReadInt16() => Read<short>();

        public ushort ReadUInt16() => Read<ushort>();

        public int ReadInt32() => Read<int>();

        public uint ReadUInt32() => Read<uint>();

        public long ReadInt64() => Read<long>();

        public ulong ReadUInt64() => Read<ulong>();

        public float ReadSingle() => Read<float>();

        public double ReadDouble() => Read<double>();

        public string ReadString()
        {
            var sizeBuffer = new byte[sizeof(int)];
            Read(sizeBuffer, 0, sizeBuffer.Length);

            var contentBuffer = new byte[BitConverter.ToInt32(sizeBuffer)];
            Read(contentBuffer, 0, contentBuffer.Length);

            return Encoding.UTF8.GetString(contentBuffer, 0, contentBuffer.Length);
        }

        private TValue Read<TValue>() where TValue : struct, IConvertible
        {
            if (typeof(TValue).IsPrimitive)
            {
                var buffer = new byte[GetTypeSize<TValue>()];

                Read(buffer, 0, buffer.Length);

                return ConvertToPrimitiveValue<TValue>(buffer);
            }

            throw new NotImplementedException($"Cannot read a {typeof(TValue)} value from the stream.");
        }

        private static int GetTypeSize<TValue>() where TValue : struct, IConvertible
        {
            return Type.GetTypeCode(typeof(TValue)) switch
            {
                TypeCode.Byte => sizeof(byte),
                TypeCode.SByte => sizeof(sbyte),
                TypeCode.Boolean => sizeof(bool),
                TypeCode.Char => sizeof(char),
                TypeCode.Int16 => sizeof(short),
                TypeCode.UInt16 => sizeof(ushort),
                TypeCode.Int32 => sizeof(int),
                TypeCode.UInt32 => sizeof(uint),
                TypeCode.Int64 => sizeof(long),
                TypeCode.UInt64 => sizeof(ulong),
                TypeCode.Single => sizeof(float),
                TypeCode.Double => sizeof(double),
                _ => throw new NotImplementedException(),
            };
        }

        private static TValue ConvertToPrimitiveValue<TValue>(byte[] buffer) where TValue : struct, IConvertible
        {
            object @object = Type.GetTypeCode(typeof(TValue)) switch
            {
                TypeCode.Byte => buffer.Single(),
                TypeCode.SByte => (sbyte)buffer.Single(),
                TypeCode.Boolean => BitConverter.ToBoolean(buffer),
                TypeCode.Char => BitConverter.ToChar(buffer),
                TypeCode.Int16 => BitConverter.ToInt16(buffer),
                TypeCode.UInt16 => BitConverter.ToUInt16(buffer),
                TypeCode.Int32 => BitConverter.ToInt32(buffer),
                TypeCode.UInt32 => BitConverter.ToUInt32(buffer),
                TypeCode.Int64 => BitConverter.ToInt64(buffer),
                TypeCode.UInt64 => BitConverter.ToUInt64(buffer),
                TypeCode.Single => BitConverter.ToSingle(buffer),
                TypeCode.Double => BitConverter.ToDouble(buffer),
                _ => throw new NotImplementedException(),
            };


            return (TValue)@object;
        }
    }
}
