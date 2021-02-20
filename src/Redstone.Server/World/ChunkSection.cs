using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Collections;
using Redstone.Common.IO;
using Redstone.Protocol.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server
{
    public class ChunkSection : IChunkSection
    {
        public const int Size = 16;
        public const int TotalChunks = Size * Size * Size;
        public const byte BitsPerBlock = 4;

        private readonly IBlock[] _blocks;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBlockFactory _blockFactory;
        private readonly CompactedLongArray _compactedBlockArray;
        //private readonly IPalette _palette;

        // Test palette
        private readonly List<int> _palette;

        public int Index { get; }

        public ChunkSection(int index, IServiceProvider serviceProvider)
        {
            if (index < 0 || index >= Size)
            {
                throw new ArgumentException($"Index '{index}' is not a valid chunk section index.", nameof(index));
            }

            Index = index;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _blockFactory = _serviceProvider.GetRequiredService<IBlockFactory>();
            _blocks = Enumerable.Repeat(default(IBlock), TotalChunks).ToArray();
            _compactedBlockArray = new CompactedLongArray(BitsPerBlock, TotalChunks);
            //_palette = new IndirectPalette(BitsPerBlock);

            _palette = new List<int>();

            Fill(BlockType.Air);
        }

        public IBlock GetBlock(int x, int y, int z) => _blocks[GetBlockIndex(x, y, z)];

        public void SetBlock(IBlock block, int x, int y, int z)
        {
            int stateId = block.State.Id;
            int blockIndex = GetBlockIndex(x, y, z);
            int palettedIndex = 0;

            if (_palette is not null)
            {
                int indexInPalette = _palette.IndexOf(stateId);
                if (indexInPalette >= 0)
                {
                    // exists
                    palettedIndex = indexInPalette;
                }
                else
                {
                    // doesn't exists
                    _palette.Add(stateId);
                    palettedIndex = _palette.Count - 1;

                    int bitsPerValue = NeededBits(palettedIndex);

                    if (bitsPerValue > _compactedBlockArray.BitsPerEntry)
                    {
                        if (bitsPerValue < 14) // move to const
                        {
                            // resize
                        }
                        else
                        {
                            // switch to global palette
                        }
                    }
                }
            }
            else
            {
                // USE GLOBAL
            }

            _compactedBlockArray[blockIndex] = palettedIndex;

            block.Position.X = x;
            block.Position.Y = y;
            block.Position.Z = z;

            _blocks[blockIndex] = block;
        }

        public short GetBlockAmount() => (short)_blocks.Count(block => block is not null && !block.IsAir);

        private int GetBlockIndex(int x, int y, int z) => (y << 8) | (z << 4) | x;

        private int NeededBits(int value) => Convert.ToString(value, 2).Length;

        private void Fill(BlockType blockType)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        SetBlock(_blockFactory.CreateBlock(blockType), x, y, z);
                    }
                }
            }
        }

        public void Serialize(MinecraftStream packet)
        {
            packet.WriteInt16(GetBlockAmount());
            packet.WriteByte(_compactedBlockArray.BitsPerEntry); // bits per block

            var bitsPerBlock = _compactedBlockArray.BitsPerEntry;

            if (_palette is not null)
            {
                packet.WriteVarInt32(_palette.Count);
                foreach (int value in _palette)
                {
                    packet.WriteVarInt32(value);
                }
            }

            int dataLength = (16 * 16 * 16) * bitsPerBlock / 64; // See tips section for an explanation of this calculation
            UInt64[] data = new UInt64[dataLength];
            uint individualValueMask = (uint)((1 << bitsPerBlock) - 1);

            for (int y = 0; y < Size; y++)
            {
                for (int z = 0; z < Size; z++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        int blockNumber = (((y * Size) + z) * Size) + x;
                        int startLong = (blockNumber * bitsPerBlock) / 64;
                        int startOffset = (blockNumber * bitsPerBlock) % 64;
                        int endLong = ((blockNumber + 1) * bitsPerBlock - 1) / 64;

                        IBlock block = GetBlock(x, y, z);
                        //BlockState state = section.GetState(x, y, z);

                        UInt64 value = (ulong)block.State.Id;
                        value &= individualValueMask;

                        data[startLong] |= (value << startOffset);

                        if (startLong != endLong)
                        {
                            data[endLong] = (value >> (64 - startOffset));
                        }
                    }
                }
            }
            // data array length
            packet.WriteVarInt32(data.Length);

            // data array
            for (int i = 0; i < data.Length; i++)
            {
                packet.WriteUInt64(data[i]);
            }


            // palette

            //if (_palette is not null)
            //{
            //    packet.WriteVarInt32(_palette.Count);
            //    foreach (int value in _palette)
            //    {
            //        packet.WriteVarInt32(value);
            //    }
            //}

            //// data array length
            //packet.WriteVarInt32(_compactedBlockArray.Length);

            //// data array
            //for (int i = 0; i < _compactedBlockArray.Storage.Length; i++)
            //{
            //    packet.WriteInt64(_compactedBlockArray.Storage[i]);
            //}
        }

        public void Serialize(IMinecraftPacket packet)
        {
            packet.WriteInt16(GetBlockAmount());
            packet.WriteByte(_compactedBlockArray.BitsPerEntry); // bits per block
            // palette

            if (_palette is not null)
            {
                packet.WriteVarInt32(_palette.Count);
                foreach (int value in _palette)
                {
                    packet.WriteVarInt32(value);
                }
            }

            // data array length
            packet.WriteVarInt32(_compactedBlockArray.Length);

            // data array
            for (int i = 0; i < _compactedBlockArray.Storage.Length; i++)
            {
                packet.WriteInt64(_compactedBlockArray.Storage[i]);
            }
        }
    }
}
