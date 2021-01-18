using Redstone.Abstractions.World;
using Redstone.Common.Structures.Blocks;
using Redstone.Protocol.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redstone.Server.World.Palettes
{
    public class IndirectPalette : IPalette
    {
        public byte BitsPerBlock { get; }

        public IndirectPalette(byte bitsPerBlock)
        {
            BitsPerBlock = bitsPerBlock;
        }

        public int GetIdFromState(BlockStateData state)
        {
            throw new NotImplementedException();
        }

        public BlockStateData GetStateFromId(int id)
        {
            throw new NotImplementedException();
        }

        public void Serialize(IMinecraftPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
