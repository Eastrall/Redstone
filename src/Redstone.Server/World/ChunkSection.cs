using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Collections;
using Redstone.Common.Exceptions;
using Redstone.Server.World.Palettes;
using System;
using System.Linq;

namespace Redstone.Server.World
{
    internal class ChunkSection : IChunkSection
    {
        public const byte DefaultBitsPerBlock = 4;
        public const int Size = 16;
        public const int MaximumBlockAmount = 4096;

        private readonly IBlock[] _blocks;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBlockFactory _blockFactory;
        private readonly IRegistry _registry;
        private readonly CompactedLongArray _blockStorage;
        private readonly IPalette _palette;

        public bool IsDirty { get; private set; }

        public int Index { get; }

        public IChunk Chunk { get; }

        public ChunkSection(IChunk chunk, int index, IServiceProvider serviceProvider)
        {
            Chunk = chunk;
            Index = index;
            _serviceProvider = serviceProvider;
            _blocks = new IBlock[MaximumBlockAmount];
            _blockFactory = _serviceProvider.GetRequiredService<IBlockFactory>();
            _registry = _serviceProvider.GetRequiredService<IRegistry>();
            _blockStorage = new CompactedLongArray(DefaultBitsPerBlock, MaximumBlockAmount);

            if (DefaultBitsPerBlock <= 8)
            {
                _palette = new BlockStatePalette(_registry, Math.Max((byte)4, DefaultBitsPerBlock));
            }
            //else
            //{
            //    _palette = new DirectBlockStatePalette(_serviceProvider.GetRequiredService<IRegistry>());
            //}

            InitializeEmptySection();
        }

        public IBlock GetBlock(int x, int y, int z)
        {
            int index = GetBlockIndex(x, y % Size, z);

            return _blocks[index];
        }

        public short GetBlockAmount()
        {
            short validBlockCount = 0;
            
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        IBlock block = GetBlock(x, y, z);

                        if (!block.IsAir)
                        {
                            validBlockCount++;
                        }
                    }
                }
            }

            return validBlockCount;
        }

        public IBlock SetBlock(BlockType blockType, int x, int y, int z)
        {
            int blockIndex = GetBlockIndex(x, y, z);
            IBlock block = _blocks.ElementAtOrDefault(blockIndex) ?? throw new BlockNotFoundException(x, y, z);

            block.SetType(blockType);

            int paletteIndex = _palette.GetState(block.State.Id);
            _blockStorage[blockIndex] = paletteIndex;
            IsDirty = true;

            return block;
        }

        public void Serialize(IMinecraftPacket packet)
        {
            packet.WriteInt16(GetBlockAmount());
            packet.WriteByte(DefaultBitsPerBlock);

            _palette.Serialize(packet);

            packet.WriteVarInt32(_blockStorage.Storage.Length);

            long[] storage = _blockStorage.Storage;
            for (int i = 0; i < storage.Length; i++)
            {
                packet.WriteInt64(storage[i]);
            }
        }

        private void InitializeEmptySection()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        int blockIndex = GetBlockIndex(x, y, z);
                        IBlock block = _blockFactory.CreateBlock(BlockType.Air, x, y, z, Chunk);

                        _blocks[blockIndex] = block;
                        _blockStorage[blockIndex] = _palette.GetState(block.State.Id);
                    }
                }
            }
        }

        public static int GetBlockIndex(int x, int y, int z) => ((y * Size) + z) * Size + x;
    }
}
