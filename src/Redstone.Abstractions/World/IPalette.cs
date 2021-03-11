using Redstone.Abstractions.Protocol;

namespace Redstone.Abstractions.World
{
    public interface IPalette
    {
        bool IsFull { get; }

        int GetIdFromState(IBlock blockState);

        IBlock GetStateFromIndex(int index);

        void Serialize(IMinecraftPacket stream);
    }
}
