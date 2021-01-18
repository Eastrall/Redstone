using Redstone.Common.Structures.Blocks;
using Redstone.Protocol.Abstractions;

namespace Redstone.Abstractions.World
{
    public interface IPalette
    {
        byte BitsPerBlock { get; }

        int GetIdFromState(BlockStateData state);

        BlockStateData GetStateFromId(int id);

        void Serialize(IMinecraftPacket packet);
    }
}
