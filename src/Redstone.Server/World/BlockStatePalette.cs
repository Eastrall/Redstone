using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;

namespace Redstone.Server.World.Palettes
{
    internal class BlockStatePalette : IPalette
    {
        private readonly int?[] _blockStates;
        private readonly IRegistry _registry;

        private int _count;

        public bool IsFull => _blockStates.Length == _count;

        public int Count => _count;

        public BlockStatePalette(IRegistry registry, byte bitCount)
        {
            _registry = registry;
            _blockStates = new int?[1 << bitCount];
        }

        public int GetState(int blockStateId)
        {
            for (var index = 0; index < Count; index++)
            {
                if (_blockStates[index] == blockStateId)
                {
                    return index;
                }
            }

            if (IsFull)
            {
                return -1;
            }

            int newStateId = AddStateToPalette(blockStateId);

            return newStateId;
        }

        public void Serialize(IMinecraftPacket stream)
        {
            stream.WriteVarInt32(_count);

            for (var i = 0; i < _count; i++)
            {
                stream.WriteVarInt32(_blockStates[i].Value);
            }
        }

        private int AddStateToPalette(int blockStateId)
        {
            int newStateId = _count;

            _blockStates[newStateId] = blockStateId;
            _count++;

            return newStateId;
        }
    }
}
