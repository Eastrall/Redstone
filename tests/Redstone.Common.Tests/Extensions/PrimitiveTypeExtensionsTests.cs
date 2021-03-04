using Redstone.Common.Extensions;
using System;
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

        [Theory]
        [InlineData(0, 0)]
        [InlineData(45, 0.785398f)]
        [InlineData(90, 1.5708f)]
        public void DegreeToRadianTest(float degreeAngle, float expectedRadianAngle)
        {
            Assert.Equal(Math.Round(expectedRadianAngle, 5), Math.Round(degreeAngle.ToRadian(), 5));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0.785398f, 45)]
        [InlineData(1.5708f, 90)]
        public void RadianToDegreeTest(float randianAngle, float expectedDegreeAngle)
        {
            Assert.Equal(Math.Round(expectedDegreeAngle, 5), Math.Round(randianAngle.ToDegree(), 0));
        }

        [Theory]
        [InlineData(100, 50, 50)]
        [InlineData(100, 20, 20)]
        [InlineData(200, 20, 40)]
        public void PercentageTest(int value, int percentage, int expectedValue)
        {
            float percentageValue = value.Percentage(percentage);

            Assert.Equal(expectedValue, percentageValue);
        }
    }
}
