using Redstone.Abstractions.Protocol;

namespace Redstone.Abstractions.World
{
    public interface IPalette
    {
        /// <summary>
        /// Gets a boolean value that indicates if the palette is full.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Gets the total amount of block states in the current palette.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the palette index of the given block state id.
        /// </summary>
        /// <param name="blockStateId">Block state id.</param>
        /// <returns>Block state index in the palette.</returns>
        int GetState(int blockStateId);

        /// <summary>
        /// Serializes the current block state palette.
        /// </summary>
        /// <param name="stream">Minecraft packet stream.</param>
        void Serialize(IMinecraftPacket stream);
    }
}
