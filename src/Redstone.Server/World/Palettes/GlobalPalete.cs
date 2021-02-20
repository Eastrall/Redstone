using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common.Structures.Blocks;
using Redstone.Protocol.Abstractions;
using System;

namespace Redstone.Server.World.Palettes
{
    public class GlobalPalete : IPalette
    {
        public const byte MaximumBitsPerBlock = 14;

        private readonly IRegistry _registry;

        public byte BitsPerBlock => MaximumBitsPerBlock;

        public GlobalPalete(IRegistry registry)
        {
            _registry = registry;
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
