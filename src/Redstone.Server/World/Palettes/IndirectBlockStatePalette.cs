using Redstone.Abstractions.World;
using Redstone.Protocol.Abstractions;
using System;

namespace Redstone.Server.World.Palettes
{
    public class IndirectBlockStatePalette : IPalette
    {
        public IBlock[] BlockStateArray { get; set; }
        public int BlockStateCount { get; set; }

        public bool IsFull => BlockStateArray.Length == BlockStateCount;

        public IndirectBlockStatePalette(byte bitCount)
        {
            BlockStateArray = new Block[1 << bitCount];
        }

        public int GetIdFromState(IBlock blockState)
        {
            for (var id = 0; id < BlockStateCount; id++)
            {
                if (BlockStateArray[id].State.Id == blockState.State.Id)
                {
                    return id;
                }
            }

            if (IsFull)
                return -1;

            // Add to palette
            var newId = BlockStateCount;
            BlockStateArray[newId] = blockState;
            BlockStateCount++;
            return newId;
        }

        public IBlock GetStateFromIndex(int index)
        {
            return index > BlockStateCount - 1 || index < 0 ? throw new IndexOutOfRangeException() : BlockStateArray[index];
        }

        public void Serialize(IMinecraftPacket stream)
        {
            stream.WriteVarInt32(BlockStateCount);

            for (var i = 0; i < BlockStateCount; i++)
            {
                stream.WriteVarInt32(BlockStateArray[i].State.Id);
            }
        }
    }
}
