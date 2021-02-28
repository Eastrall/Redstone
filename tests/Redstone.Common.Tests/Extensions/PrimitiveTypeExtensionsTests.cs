using Redstone.Common.Extensions;
using Xunit;

namespace Redstone.Common.Tests.Extensions
{
    public class PrimitiveTypeExtensionsTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(3, 2)]
        [InlineData(10, 4)]
        public void IntegerNeededBitsTest(int value, int expectedBits)
        {
            Assert.Equal(expectedBits, value.NeededBits());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(3, 2)]
        [InlineData(10, 4)]
        public void ShortNeededBitsTest(short value, int expectedBits)
        {
            Assert.Equal(expectedBits, value.NeededBits());
        }
    }
}
