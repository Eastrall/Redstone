using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;

namespace Redstone.Server.World.Palettes
{
    internal class BlockStatePalette : IPalette
    {
        private readonly IRegistry _registry;

        public bool IsFull => throw new System.NotImplementedException();

        public BlockStatePalette(IRegistry registry)
        {
            _registry = registry;
        }

        public int GetIdFromState(IBlock blockState)
        {
            throw new System.NotImplementedException();
        }

        public IBlock GetStateFromIndex(int index)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(IMinecraftPacket stream)
        {
            throw new System.NotImplementedException();
        }
    }
}
