using Redstone.Abstractions;
using Redstone.Abstractions.World;

namespace Redstone.Server.World.Blocks
{
    public class Block : IBlock
    {
        public Position Position { get; }

        protected Block()
        {
            Position = new Position();
        }
    }
}
