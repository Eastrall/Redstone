using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using System.Linq;

namespace Redstone.Server.World.Palettes
{
    public class DirectBlockStatePalette : IPalette
    {
        private readonly IRegistry _registry;

        public bool IsFull => false;

        public DirectBlockStatePalette(IRegistry registry)
        {
            _registry = registry;
        }

        public int GetIdFromState(IBlock block) => block.State.Id;

        public IBlock GetStateFromIndex(int index)
        {
            return new Block(_registry.Blocks.FirstOrDefault(x => x.States.Any(x => x.Id == index)));
        }

        public void Serialize(IMinecraftPacket _)
        {
            // Nothing to do.
        }
    }

}
