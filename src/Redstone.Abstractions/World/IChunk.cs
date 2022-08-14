using Redstone.Abstractions.Protocol;
using Redstone.Common;
using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.World;

/// <summary>
/// Provides a mechanism to manage a Minecraft chunk.
/// </summary>
public interface IChunk
{
    /// <summary>
    /// Gets the chunk parent region.
    /// </summary>
    IRegion Region { get; }

    /// <summary>
    /// Gets a boolean value that indicates if the current chunk is dirty.
    /// </summary>
    bool IsDirty { get; }

    /// <summary>
    /// Gets the chunk X coordinate within its region.
    /// </summary>
    int X { get; }

    /// <summary>
    /// Gets the chunk Z coordinate within the its region.
    /// </summary>
    int Z { get; }

    /// <summary>
    /// Gets the number of solid blocks inside the chunk.
    /// </summary>
    int BlockAmount { get; }

    /// <summary>
    /// Gets the chunk heightmap.
    /// </summary>
    IEnumerable<long> Heightmap { get; }

    /// <summary>
    /// Gets the chunk world surface heightmap.
    /// </summary>
    IEnumerable<long> WorldSurfaceHeightmap { get; }

    /// <summary>
    /// Gets the chunk sections.
    /// </summary>
    IEnumerable<IChunkSection> Sections { get; }

    /// <summary>
    /// Generates the chunk heightmap.
    /// </summary>
    void GenerateHeightMap();

    /// <summary>
    /// Gets the block at the given coordinates.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the Y coordinate is negative or grather than 255.
    /// </exception>
    /// <returns>Block.</returns>
    IBlock GetBlock(int x, int y, int z);

    /// <summary>
    /// Gets a chunk section at the given section index.
    /// </summary>
    /// <param name="sectionIndex">Chunk section index.</param>
    /// <returns>Chunk section if found.</returns>
    IChunkSection GetSection(int sectionIndex);

    /// <summary>
    /// Sets a block at the given coordinates.
    /// </summary>
    /// <param name="blockType">Block type.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <returns>The block placed at the given position.</returns>
    IBlock SetBlock(BlockType blockType, int x, int y, int z);

    /// <summary>
    /// Serializes the current chunk into the given packet stream.
    /// </summary>
    /// <param name="packet">Minecraft packet stream.</param>
    /// <param name="fullChunk">
    /// Boolean value that indicates if the method should serialize the hole chunk or not.
    /// </param>
    void Serialize(IMinecraftPacket packet, bool fullChunk = false);
}
