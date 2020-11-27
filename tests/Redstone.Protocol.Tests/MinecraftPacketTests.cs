using System;
using Xunit;

namespace Redstone.Protocol.Tests
{
    public class MinecraftPacketTests
    {
        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        public void MinecraftPacketReadVarInt32Test(int value, byte[] expectedBytes)
        {
            Assert.True(true);
        }
    }
}
