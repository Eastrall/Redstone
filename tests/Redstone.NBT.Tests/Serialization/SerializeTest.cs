using Redstone.NBT.Attributes;
using Redstone.NBT.Serialization;
using Redstone.NBT.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redstone.NBT.Tests.Serialization
{
    public class SerializeTest
    {
        [Fact]
        public void SerializeCompoundTest()
        {
            var element = new CustomNbtElement
            {
                Id = 42,
                Name = "Hello",
                Temperature = 32.64f
            };

            NbtCompound compound = NbtSerializer.SerializeCompound(element);

            Assert.NotNull(compound);

            NbtTag idTag = compound.Get("id");
            Assert.NotNull(idTag);
            Assert.IsType<NbtInt>(idTag);
            Assert.Equal(element.Id, idTag.IntValue);
        }
    }

    public class CustomNbtElement
    {
        [NbtElement(NbtTagType.Int, "id")]
        public int Id { get; set; }

        [NbtElement(NbtTagType.String, "name")]
        public string Name { get; set; }

        [NbtElement(NbtTagType.Float, "temperature")]
        public float Temperature { get; set; }
    }
}
