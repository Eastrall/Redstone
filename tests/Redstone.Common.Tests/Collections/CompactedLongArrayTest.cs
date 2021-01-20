using Bogus;
using Redstone.Common.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Redstone.Common.Tests.Collections
{
    public class CompactedLongArrayTest
    {
        [Theory]
        [InlineData(4, 4096, 4)]
        [InlineData(4, 4096, 8)]
        [InlineData(4, 4096, 10)]
        [InlineData(4, 4096, 12)]
        [InlineData(4, 4096, 15)]
        [InlineData(8, 1024, 4)]
        [InlineData(8, 1024, 8)]
        [InlineData(8, 1024, 10)]
        [InlineData(8, 1024, 12)]
        [InlineData(8, 1024, 15)]
        public void Test(byte bitsPerEntry, int capacity, int value)
        {
            var array = new CompactedLongArray(bitsPerEntry, capacity);

            for (int i = 0; i < capacity; i++)
            {
                array[i] = value;
            }

            for (int i = 0; i < capacity; i++)
            {
                Assert.Equal(value, array[i]);
            }
        }
    }
}
