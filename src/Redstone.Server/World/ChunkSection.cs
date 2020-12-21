using Redstone.Abstractions;
using Redstone.Abstractions.World;
using Redstone.Server.World.Blocks;
using System.Linq;

namespace Redstone.Server
{
    public class ChunkSection : IChunkSection
    {
        public const int Size = 16;
        public const int TotalChunks = Size * Size * Size;

        private readonly IBlock[] _blocks;

        public int Index { get; }

        public ChunkSection(int index)
        {
            Index = index;
            _blocks = Enumerable.Repeat(default(IBlock), TotalChunks).ToArray();

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int z = 0; z < Size; z++)
                    {
                        IBlock block = new AirBlock
                        {
                            Position = new Position(x, y, z)
                        };

                        _blocks[GetBlockIndex(x, y, z)] = block;
                    }
                }
            }
        }

        public IBlock GetBlock(int x, int y, int z) => _blocks[GetBlockIndex(x, y, z)];

        public void SetBlock(int x, int y, int z, IBlock block)
        {
            block.Position.X = x;
            block.Position.Y = y;
            block.Position.Z = z;

            _blocks[GetBlockIndex(x, y, z)] = block;
        }

        private int GetBlockIndex(int x, int y, int z) => z + Size * (y + Size * x);
    }
}
