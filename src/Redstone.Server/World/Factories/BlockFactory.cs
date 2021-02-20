using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Structures.Blocks;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Redstone.Server.World.Factories
{
    [Injectable(ServiceLifetime.Singleton)]
    public class BlockFactory : IBlockFactory
    {
        private readonly IRegistry _registry;
        private readonly ConcurrentDictionary<BlockType, BlockData> _blocksDatas;

        public BlockFactory(IRegistry registry)
        {
            _registry = registry;
            _blocksDatas = new ConcurrentDictionary<BlockType, BlockData>();
        }

        public IBlock CreateBlock(BlockType blockType)
        {
            if (!_blocksDatas.TryGetValue(blockType, out BlockData blockData))
            {
                blockData = _registry.Blocks.SingleOrDefault(x => x.Type == blockType);

                if (blockData is null)
                {
                    throw new InvalidOperationException($"Failed to find block data for type: '{blockType}'");
                }

                _blocksDatas.TryAdd(blockType, blockData);
            }

            return new Block(blockData);
        }
    }
}
