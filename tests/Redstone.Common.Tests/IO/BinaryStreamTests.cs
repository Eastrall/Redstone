using Bogus;
using Redstone.Common.IO;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Redstone.Common.Tests.IO
{
    public class BinaryStreamTests
    {
        private readonly Faker _faker;

        public BinaryStreamTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void ReadByteTest()
        {
            var value = _faker.Random.Byte();

            using var stream = new BinaryStream(new[] { value });

            var streamValue = stream.ReadByte();

            Assert.Equal(value, (byte)streamValue);
        }

        [Fact]
        public void ReadCharTest()
        {
            var value = _faker.Random.Char();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadChar();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadBooleanTest()
        {
            var value = _faker.Random.Bool();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadBoolean();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadInt16Test()
        {
            var value = _faker.Random.Short();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadInt16();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadUInt16Test()
        {
            var value = _faker.Random.UShort();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadUInt16();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadInt32Test()
        {
            var value = _faker.Random.Int();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadInt32();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadUInt32Test()
        {
            var value = _faker.Random.UInt();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadUInt32();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadInt64Test()
        {
            var value = _faker.Random.Long();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadInt64();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadUInt64Test()
        {
            var value = _faker.Random.ULong();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadUInt64();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadSingleTest()
        {
            var value = _faker.Random.Float();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadSingle();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadDoubleTest()
        {
            var value = _faker.Random.Double();
            var buffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadDouble();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void ReadStringTest()
        {
            var value = _faker.Lorem.Sentence();
            var buffer = BitConverter.GetBytes(value.Length).Concat(Encoding.UTF8.GetBytes(value)).ToArray();

            using var stream = new BinaryStream(buffer);

            var streamValue = stream.ReadString();

            Assert.Equal(value, streamValue);
        }

        [Fact]
        public void WriteByteTest()
        {
            var value = _faker.Random.Byte();
            var expectedBuffer = BitConverter.GetBytes((byte)value);

            using var stream = new BinaryStream();

            stream.WriteByte(value);

            Assert.Equal(expectedBuffer.Take(1).ToArray(), stream.Buffer);
        }

        [Fact]
        public void WriteSByteTest()
        {
            var value = _faker.Random.SByte();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteSByte(value);

            Assert.Equal(expectedBuffer.Take(1).ToArray(), stream.Buffer);
        }

        [Fact]
        public void WriteBooleanTest()
        {
            var value = _faker.Random.Bool();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteBoolean(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteCharTest()
        {
            var value = _faker.Random.Char();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteChar(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteInt16Test()
        {
            var value = _faker.Random.Short();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteInt16(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteUInt16Test()
        {
            var value = _faker.Random.UShort();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteUInt16(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteInt32Test()
        {
            var value = _faker.Random.Int();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteInt32(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteUInt32Test()
        {
            var value = _faker.Random.UInt();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteUInt32(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteInt64Test()
        {
            var value = _faker.Random.Long();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteInt64(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteUInt64Test()
        {
            var value = _faker.Random.ULong();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteUInt64(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteSingleTest()
        {
            var value = _faker.Random.Float();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteSingle(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteDoubleTest()
        {
            var value = _faker.Random.Double();
            var expectedBuffer = BitConverter.GetBytes(value);

            using var stream = new BinaryStream();

            stream.WriteDouble(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }

        [Fact]
        public void WriteStringTest()
        {
            var value = _faker.Lorem.Sentence();
            var expectedBuffer = BitConverter.GetBytes(value.Length).Concat(Encoding.UTF8.GetBytes(value)).ToArray();

            using var stream = new BinaryStream();

            stream.WriteString(value);

            Assert.Equal(expectedBuffer, stream.Buffer);
        }
    }
}
