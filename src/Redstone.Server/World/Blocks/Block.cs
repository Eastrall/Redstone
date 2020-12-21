using Redstone.Abstractions;
using Redstone.Abstractions.World;
using Redstone.Common;
using System.Diagnostics;

namespace Redstone.Server.World.Blocks
{
    [DebuggerDisplay("{Type}: {Position}")]
    public abstract class Block : IBlock
    {
        public bool IsAir => Type is BlockType.Air or BlockType.CaveAir or BlockType.VoidAir;

        public abstract BlockType Type { get; }

        public Position Position { get; init; }

        protected Block()
        {
            Position = new Position();
        }
    }
}
