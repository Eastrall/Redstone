namespace Redstone.Abstractions.World
{
    public interface IChunkSection
    {
        int Index { get; }

        IBlock GetBlock(int x, int y, int z);

        void SetBlock(int x, int y, int z, IBlock block);
    }
}
