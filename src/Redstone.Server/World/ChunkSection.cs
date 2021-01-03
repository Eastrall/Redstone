using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions;
using Redstone.Abstractions.World;
using Redstone.Protocol.Abstractions;
using Redstone.Server.World.Blocks;
using System;
using System.Linq;

namespace Redstone.Server
{
    public class ChunkSection : IChunkSection
    {
        public const int Size = 16;
        public const int TotalChunks = Size * Size * Size;

        private readonly IBlock[] _blocks;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBlockFactory _blockFactory;

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

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        IBlock block = _blockFactory.CreateBlock<AirBlock>();

                        block.Position.X = x;
                        block.Position.Y = y;
                        block.Position.Z = z;

                        _blocks[GetBlockIndex(x, y, z)] = block;
                    }
                }
            }
        }

        public IBlock GetBlock(int x, int y, int z) => _blocks[GetBlockIndex(x, y, z)];

        public void SetBlock(IBlock block, int x, int y, int z)
        {
            block.Position.X = x;
            block.Position.Y = y;
            block.Position.Z = z;

            _blocks[GetBlockIndex(x, y, z)] = block;
        }

        public int GetBlockAmount() => _blocks.Count(block => block is not null && !block.IsAir);

        private int GetBlockIndex(int x, int y, int z) => z + Size * (y + Size * x);

        public void Serialize(IMinecraftPacket packet)
        {
            packet.WriteVarInt32(GetBlockAmount());
            packet.WriteByte(4); // bits per block
            // palette
            // data array length
            // data array
        }
    }
}
