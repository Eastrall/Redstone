using Redstone.Abstractions;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Structures.Blocks;
using System;
using System.Diagnostics;
using System.Linq;

namespace Redstone.Server.World.Blocks
{
    [DebuggerDisplay("{Type}: {Position}")]
    public abstract class Block : IBlock
    {
        private readonly BlockData _blockData;

        public bool IsAir => Type is BlockType.Air or BlockType.CaveAir or BlockType.VoidAir;

        public BlockType Type => _blockData.Type;

        public Position Position { get; }

        public BlockStateData State { get; }

        protected Block(BlockData blockData)
        {
            _blockData = blockData ?? throw new ArgumentNullException(nameof(blockData), "Cannot create a block with no block data.");
            Position = new Position();
            State = _blockData.States.FirstOrDefault(x => x.IsDefault);

            if (State is null)
            {
                throw new InvalidOperationException($"Failed to initialize block. Cannot find default state for block data: {_blockData.Name}");
            }
        }
    }
}
