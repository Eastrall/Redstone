using Bogus;
using Redstone.NBT.Serialization;
using Redstone.NBT.Tags;
using System;
using Xunit;

namespace Redstone.NBT.Tests.Serialization;

public class SerializeTest
{
    [Fact]
    public void SerializeFlatCompoundTest()
    {
        var faker = new Faker();
        var element = new CustomFlatNbtElement
        {
            ByteValue = faker.Random.Byte(),
            ShortValue = faker.Random.Short(),
            IntegerValue = faker.Random.Int(),
            FloatValue = faker.Random.Float(),
            LongValue = faker.Random.Long(),
            DoubleValue = faker.Random.Double(),
            StringValue = faker.Lorem.Sentence(),
        };

        NbtCompound compound = NbtSerializer.SerializeCompound(element);

        Assert.NotNull(compound);

        AssertTag<NbtByte, byte>(compound, "byte_value", element.ByteValue, x => x.ByteValue);
        AssertTag<NbtShort, short>(compound, "short_value", element.ShortValue, x => x.ShortValue);
        AssertTag<NbtInt, int>(compound, "integer_value", element.IntegerValue, x => x.IntValue);
        AssertTag<NbtFloat, float>(compound, "float_value", element.FloatValue, x => x.FloatValue);
        AssertTag<NbtLong, long>(compound, "long_value", element.LongValue, x => x.LongValue);
        AssertTag<NbtDouble, double>(compound, "double_value", element.DoubleValue, x => x.DoubleValue);
        AssertTag<NbtString, string>(compound, "string_value", element.StringValue, x => x.StringValue);
    }

    [Fact]
    public void SerializeComplexCompoundTest()
    {
        var faker = new Faker();
        var element = new CustomComplexNbtElement
        {
            IntegerValue = faker.Random.Int(),
            FlatElement = new CustomFlatNbtElement
            {
                ByteValue = faker.Random.Byte(),
                ShortValue = faker.Random.Short(),
                IntegerValue = faker.Random.Int(),
                FloatValue = faker.Random.Float(),
                LongValue = faker.Random.Long(),
                DoubleValue = faker.Random.Double(),
                StringValue = faker.Lorem.Sentence(),
            }
        };

        NbtCompound compound = NbtSerializer.SerializeCompound(element);

        Assert.NotNull(compound);
        AssertTag<NbtInt, int>(compound, "integer_value", element.IntegerValue, x => x.IntValue);

        NbtCompound innerCompound = compound.Get("flat_element_compound") as NbtCompound;

        Assert.NotNull(innerCompound);
        AssertTag<NbtByte, byte>(innerCompound, "byte_value", element.FlatElement.ByteValue, x => x.ByteValue);
        AssertTag<NbtShort, short>(innerCompound, "short_value", element.FlatElement.ShortValue, x => x.ShortValue);
        AssertTag<NbtInt, int>(innerCompound, "integer_value", element.FlatElement.IntegerValue, x => x.IntValue);
        AssertTag<NbtFloat, float>(innerCompound, "float_value", element.FlatElement.FloatValue, x => x.FloatValue);
        AssertTag<NbtLong, long>(innerCompound, "long_value", element.FlatElement.LongValue, x => x.LongValue);
        AssertTag<NbtDouble, double>(innerCompound, "double_value", element.FlatElement.DoubleValue, x => x.DoubleValue);
        AssertTag<NbtString, string>(innerCompound, "string_value", element.FlatElement.StringValue, x => x.StringValue);
    }

    private void AssertTag<TTag, TValue>(NbtCompound compound, string name, TValue expectedValue, Func<NbtTag, TValue> getValueFunction)
    {
        NbtTag tag = compound.Get(name);
        Assert.NotNull(tag);
        Assert.IsType<TTag>(tag);
        Assert.Equal(expectedValue, getValueFunction(tag));
    }
}
