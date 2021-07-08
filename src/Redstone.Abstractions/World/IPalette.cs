using Redstone.Abstractions.Protocol;
using System;

namespace Redstone.Abstractions.World
{
    /// <summary>
    /// Provides a mechanism to manage a Minecraft block state palete.
    /// </summary>
    /// <remarks>
    /// More information: https://wiki.vg/Chunk_Format#Global_and_section_palettes
    /// </remarks>
    public interface IPalette
    {
        /// <summary>
        /// Event fired when the palette has been resized.
        /// </summary>
        event EventHandler<int> Resized;

        /// <summary>
        /// Gets a boolean value that indicates if the palette is full.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the global palette is being used.
        /// </summary>
        bool IsUsingGlobalPalette { get; }

        /// <summary>
        /// Gets the number of bits allocated per blocks.
        /// </summary>
        int BitsPerBlock { get; }

        /// <summary>
        /// Gets the total amount of block states in the current palette.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the palette index of the given block state id.
        /// </summary>
        /// <param name="blockStateId">Block state id.</param>
        /// <returns>Block state index in the palette.</returns>
        int SetState(int blockStateId);

        /// <summary>
        /// Serializes the current block state palette.
        /// </summary>
        /// <param name="stream">Minecraft packet stream.</param>
        void Serialize(IMinecraftPacket stream);
    }
}
