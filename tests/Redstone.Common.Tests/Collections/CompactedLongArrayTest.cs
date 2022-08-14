using Bogus;
using Redstone.Common.Collections;
using System.Collections.Generic;
using Xunit;

namespace Redstone.Common.Tests.Collections;

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
    public void CompactedLongArrayIndexerTest(byte bitsPerEntry, int capacity, int value)
    {
        var array = new CompactedLongArray(bitsPerEntry, capacity);

        Assert.Equal(capacity, array.Length);
        Assert.Equal(bitsPerEntry, array.BitsPerEntry);

        for (int i = 0; i < capacity; i++)
        {
            array[i] = value;
        }

        for (int i = 0; i < capacity; i++)
        {
            Assert.Equal(value, array[i]);
        }
    }

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
    public void CompactedLongArrayMethodsTest(byte bitsPerEntry, int capacity, int value)
    {
        var array = new CompactedLongArray(bitsPerEntry, capacity);

        Assert.Equal(capacity, array.Length);
        Assert.Equal(bitsPerEntry, array.BitsPerEntry);

        for (int i = 0; i < capacity; i++)
        {
            array.Set(i, value);
        }

        for (int i = 0; i < capacity; i++)
        {
            Assert.Equal(value, array.Get(i));
        }
    }

    [Theory]
    [InlineData(4, 5)]
    [InlineData(6, 8)]
    [InlineData(10, 11)]
    [InlineData(8, 6)]
    public void ResizeCompactedLongArrayTest(byte bitsPerEntry, byte newBitsPerEntry)
    {
        var faker = new Faker();
        var array = new CompactedLongArray(bitsPerEntry, 4096);
        var listOfValues = new List<int>();

        for (int i = 0; i < 4096; i++)
        {
            int value = faker.Random.Int(0, 15);

            listOfValues.Add(value);
            array.Set(i, value);
        }

        Assert.Equal(bitsPerEntry, array.BitsPerEntry);
        array.Resize(newBitsPerEntry);
        Assert.Equal(newBitsPerEntry, array.BitsPerEntry);

        for (int i = 0; i < 4096; i++)
        {
            int value = array.Get(i);
            int savedValue = listOfValues[i];

            Assert.Equal(savedValue, value);
        }
    }
}
