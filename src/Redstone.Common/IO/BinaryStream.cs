using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Redstone.Common.IO
{
    public class BinaryStream : MemoryStream
    {
        protected virtual bool ReverseIfLittleEndian => false;

        public virtual byte[] Buffer => TryGetBuffer(out ArraySegment<byte> buffer) ? buffer.ToArray() : Array.Empty<byte>();

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

        public virtual void WriteSByte(sbyte value) => WriteByte((byte)value);

        public virtual void WriteBoolean(bool value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteChar(char value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteInt16(short value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteUInt16(ushort value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteInt32(int value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteUInt32(uint value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteInt64(long value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteUInt64(ulong value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteSingle(float value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteDouble(double value) => InternalWriteBytes(BitConverter.GetBytes(value));

        public virtual void WriteBytes(byte[] values) => Write(values, 0, values.Length);

        public virtual void WriteString(string value)
        {
            WriteInt32(value.Length);
            WriteBytes(Encoding.UTF8.GetBytes(value));
        }

        public virtual new byte ReadByte() => (byte)base.ReadByte();

        public virtual sbyte ReadSByte() => InternalReadValue<sbyte>();

        public virtual char ReadChar() => InternalReadValue<char>();

        public virtual bool ReadBoolean() => InternalReadValue<bool>();

        public virtual short ReadInt16() => InternalReadValue<short>();

        public virtual ushort ReadUInt16() => InternalReadValue<ushort>();

        public virtual int ReadInt32() => InternalReadValue<int>();

        public virtual uint ReadUInt32() => InternalReadValue<uint>();

        public virtual long ReadInt64() => InternalReadValue<long>();

        public virtual ulong ReadUInt64() => InternalReadValue<ulong>();

        public virtual float ReadSingle() => InternalReadValue<float>();

        public virtual double ReadDouble() => InternalReadValue<double>();

        public virtual string ReadString()
        {
            int stringLength = ReadInt32();
            byte[] stringData = ReadBytes(stringLength);

            return Encoding.UTF8.GetString(stringData);
        }

        public virtual byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];

            Read(bytes, 0, count);

            return bytes;
        }

        private void InternalWriteBytes(byte[] values)
        {
            if (BitConverter.IsLittleEndian && ReverseIfLittleEndian)
            {
                Array.Reverse(values);
            }

            WriteBytes(values);
        }

        private TValue InternalReadValue<TValue>() where TValue : struct, IConvertible
        {
            if (typeof(TValue).IsPrimitive)
            {
                var buffer = new byte[GetTypeSize<TValue>()];

                Read(buffer, 0, buffer.Length);

                if (BitConverter.IsLittleEndian && ReverseIfLittleEndian)
                {
                    Array.Reverse(buffer);
                }

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
