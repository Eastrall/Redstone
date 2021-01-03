using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Structures.Blocks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redstone.Server.World.Factories
{
    [Injectable(ServiceLifetime.Singleton)]
    public class BlockFactory : IBlockFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRegistry _registry;
        private readonly ConcurrentDictionary<Type, BlockData> _blockDatas;
        private readonly IDictionary<Type, string> _blockNames;

        public BlockFactory(IServiceProvider serviceProvider, IRegistry registry)
        {
            _serviceProvider = serviceProvider;
            _registry = registry;
            _blockDatas = new ConcurrentDictionary<Type, BlockData>();
            _blockNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetCustomAttribute<BlockNameAttribute>() != null)
                .Select(x => new
                {
                    BlockName = x.GetCustomAttribute<BlockNameAttribute>().Name,
                    Type = x
                })
                .ToDictionary(x => x.Type, x => x.BlockName);
        }

        public TBlock CreateBlock<TBlock>() where TBlock : class, IBlock
        {
            if (!_blockDatas.TryGetValue(typeof(TBlock), out BlockData blockData))
            {
                blockData = _registry.Blocks.FirstOrDefault(x => x.Name == GetBlockNameByType<TBlock>());

                if (blockData is null)
                {
                    throw new InvalidOperationException($"Failed to find block data with name: ''");
                }

                _blockDatas.TryAdd(typeof(TBlock), blockData);
            }

            return ActivatorUtilities.CreateInstance<TBlock>(_serviceProvider, blockData);
        }

        public TBlock CreateBlock<TBlock>(int x, int y, int z) where TBlock : class, IBlock
        {
            TBlock block = CreateBlock<TBlock>();

            block.Position.X = x;
            block.Position.Y = y;
            block.Position.Z = z;

            return block;
        }

        private string GetBlockNameByType<TBlockType>()
            => _blockNames.TryGetValue(typeof(TBlockType), out string blockName)
                ? blockName
                : throw new KeyNotFoundException($"Failed to find block name with type '{typeof(TBlockType).Name}'.");
    }
}
