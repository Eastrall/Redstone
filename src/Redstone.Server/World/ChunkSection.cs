using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Collections;
using Redstone.Server.World.Palettes;
using System;

namespace Redstone.Server.World
{
    public class ChunkSection : IChunkSection
    {
        public const byte DefaultBitsPerBlock = 4;
        public const int Size = 16;
        public const int MaximumBlockAmount = 4096;

        private readonly IServiceProvider _serviceProvider;
        private readonly IBlockFactory _blockFactory;
        private readonly CompactedLongArray _blockStorage;
        private readonly IPalette _palette;

        public int Index { get; }

        public ChunkSection(int index, IServiceProvider serviceProvider)
        {
            Index = index;
            _serviceProvider = serviceProvider; 
            _blockFactory = _serviceProvider.GetRequiredService<IBlockFactory>();
            _blockStorage = new CompactedLongArray(DefaultBitsPerBlock, MaximumBlockAmount);

            if (DefaultBitsPerBlock <= 8)
            {
                _palette = new IndirectBlockStatePalette(Math.Max((byte)4, DefaultBitsPerBlock));
            }
            else
            {
                _palette = new DirectBlockStatePalette(_serviceProvider.GetRequiredService<IRegistry>());
            }

            FillWithAir();
        }

        public IBlock GetBlock(int x, int y, int z)
        {
            y %= Size;
            int storageId = _blockStorage[GetBlockIndex(x, y, z)];

            return _palette.GetStateFromIndex(storageId);
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

        public void SetBlock(IBlock block, int x, int y, int z)
        {
            y %= Size;
            var blockIndex = GetBlockIndex(x, y, z);

            int paletteIndex = _palette.GetIdFromState(block);

            _blockStorage[blockIndex] = paletteIndex;
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

        private void FillWithAir()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        SetBlock(_blockFactory.CreateBlock(BlockType.Air), x, y, z);
                    }
                }
            }
        }

        public static int GetBlockIndex(int x, int y, int z) => ((y * Size) + z) * Size + x;
    }
}
