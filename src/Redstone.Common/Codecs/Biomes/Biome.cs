using Redstone.NBT;
using Redstone.NBT.Serialization;
using System.Diagnostics;

namespace Redstone.Common.Codecs.Biomes
{
    [DebuggerDisplay("{Name} (Id = {Id})")]
    public class Biome
    {
        [NbtElement(NbtTagType.Int, "id")]
        public int Id { get; set; }

        [NbtElement(NbtTagType.String, "name")]
        public string Name { get; set; }

        [NbtElement(NbtTagType.Compound, "element")]
        public BiomeElement Element { get; set; }
    }
}
