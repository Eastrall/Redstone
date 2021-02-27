using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Redstone.Protocol.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="MinecraftPacket"/>.
    /// </summary>
    /// <remarks>
    /// All tests cases are from https://wiki.vg/Protocol#VarInt_and_VarLong
    /// Thank you to the authors.
    /// </remarks>
    public class MinecraftPacketTests
    {
        public static IEnumerable<object[]> IntegerValues => new List<object[]>
        {
            new object[] { 0, new byte[] { 0x00 } },
            new object[] { 1, new byte[] { 0x01 } },
            new object[] { 2, new byte[] { 0x02 } },
            new object[] { 127, new byte[] { 0x7f } },
            new object[] { 128, new byte[] { 0x80, 0x01 } },
            new object[] { 255, new byte[] { 0xff, 0x01 } },
            new object[] { 2097151, new byte[] { 0xff, 0xff, 0x7f } },
            new object[] { 2147483647, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x07 } },
            new object[] { -1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x0f } },
            new object[] { -2147483648, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x08 } },
        };

        public static IEnumerable<object[]> LongValues => new List<object[]>
        {
            new object[] { 0, new byte[] { 0x00 } },
            new object[] { 1, new byte[] { 0x01 } },
            new object[] { 2, new byte[] { 0x02 } },
            new object[] { 127, new byte[] { 0x7f } },
            new object[] { 128, new byte[] { 0x80, 0x01 } },
            new object[] { 255, new byte[] { 0xff, 0x01 } },
            new object[] { 2097151, new byte[] { 0xff, 0xff, 0x7f } },
            new object[] { 2147483647, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x07 } },
            new object[] { 9223372036854775807, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f } },
            new object[] { -1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01 } },
            new object[] { -2147483648, new byte[] { 0x80, 0x80, 0x80, 0x80, 0xf8, 0xff, 0xff, 0xff, 0xff, 0x01 } },
            new object[] { -9223372036854775808, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01 } },
        };

        public static IEnumerable<object[]> UUIDValues => new List<object[]>
        {
            new object[] { new Guid("4bcb34dd-bf02-4637-a89d-38aa94d64d28"), new byte[] { 0xDD, 0x34, 0xCB, 0x4B, 0x02, 0xBF, 0x37, 0x46, 0xA8, 0x9D, 0x38, 0xAA, 0x94, 0xD6, 0x4D, 0x28 } },
            new object[] { new Guid("d09cc6f7-ce7d-40d3-82da-1be0b8a60b7e"), new byte[] { 0xF7, 0xC6, 0x9C, 0xD0, 0x7D, 0xCE, 0xD3, 0x40, 0x82, 0xDA, 0x1B, 0xE0, 0xB8, 0xA6, 0x0B, 0x7E } },
            new object[] { new Guid("c3560b95-96cf-4e89-92fd-1c14e3e472aa"), new byte[] { 0x95, 0x0B, 0x56, 0xC3, 0xCF, 0x96, 0x89, 0x4E, 0x92, 0xFD, 0x1C, 0x14, 0xE3, 0xE4, 0x72, 0xAA } },
            new object[] { new Guid("54704df9-b12b-4d16-8522-a5ec3b5fef67"), new byte[] { 0xF9, 0x4D, 0x70, 0x54, 0x2B, 0xB1, 0x16, 0x4D, 0x85, 0x22, 0xA5, 0xEC, 0x3B, 0x5F, 0xEF, 0x67 } },
            new object[] { new Guid("80371013-c40c-4dad-9b6e-92b7f7096a99"), new byte[] { 0x13, 0x10, 0x37, 0x80, 0x0C, 0xC4, 0xAD, 0x4D, 0x9B, 0x6E, 0x92, 0xB7, 0xF7, 0x09, 0x6A, 0x99 } },
            new object[] { new Guid("36d8e5c4-a174-4a58-8371-e2044e6c10de"), new byte[] { 0xC4, 0xE5, 0xD8, 0x36, 0x74, 0xA1, 0x58, 0x4A, 0x83, 0x71, 0xE2, 0x04, 0x4E, 0x6C, 0x10, 0xDE } },
            new object[] { new Guid("a6f1fb52-74d5-4e33-a3ac-29d767d7bbc2"), new byte[] { 0x52, 0xFB, 0xF1, 0xA6, 0xD5, 0x74, 0x33, 0x4E, 0xA3, 0xAC, 0x29, 0xD7, 0x67, 0xD7, 0xBB, 0xC2 } }
        };

        [Theory]
        [MemberData(nameof(IntegerValues))]
        public void MinecraftPacketReadVarInt32Test(int expectedValue, byte[] packetContent)
        {
            using var packet = new MinecraftPacket(0, packetContent);

            int value = packet.ReadVarInt32();

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [MemberData(nameof(LongValues))]
        public void MinecraftPacketReadVarInt64Test(long expectedValue, byte[] packetContent)
        {
            using var packet = new MinecraftPacket(0, packetContent);

            long value = packet.ReadVarInt64();

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [MemberData(nameof(UUIDValues))]
        public void MinecraftPacketReadUUIDTest(Guid expectedValue, byte[] packetContent)
        {
            using var packet = new MinecraftPacket(0, packetContent);

            Guid value = packet.ReadUUID();

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [MemberData(nameof(IntegerValues))]
        public void MinecraftPacketWriteVarInt32Test(int valueToWrite, byte[] expectedContent)
        {
            using var packet = new MinecraftPacket(0);

            packet.WriteVarInt32(valueToWrite);

            Assert.Equal(expectedContent, packet.BaseBuffer);
        }

        [Theory]
        [MemberData(nameof(LongValues))]
        public void MinecraftPacketWriteVarInt64Test(long valueToWrite, byte[] expectedContent)
        {
            using var packet = new MinecraftPacket(0);

            packet.WriteVarInt64(valueToWrite);

            Assert.Equal(expectedContent, packet.BaseBuffer);
        }

        [Theory]
        [MemberData(nameof(UUIDValues))]
        public void MinecraftPacketWriteUUIDTest(Guid valueToWrite, byte[] expectedContent)
        {
            using var packet = new MinecraftPacket(0);

            packet.WriteUUID(valueToWrite);

            // FIX: little/big Endian
            if (BitConverter.IsLittleEndian)
            {
                byte[] firstLong = expectedContent.Take(8).Reverse().ToArray();
                byte[] secondLong = expectedContent.Skip(8).Reverse().ToArray();

                expectedContent = firstLong.Concat(secondLong).ToArray();
            }

            Assert.Equal(expectedContent, packet.BaseBuffer);
        }
    }
}
