﻿using Redstone.Abstractions.World;
using Redstone.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server.World;

public class Region : IRegion
{
    public const int Size = 512;
    public const int ChunkAmount = Size / Chunk.Size;
    
    private readonly IChunk[] _chunks;
    private readonly IServiceProvider _serviceProvider;

    public IWorldMap WorldMap { get; }

    public int X { get; }

    public int Z { get; }

    public IEnumerable<IChunk> Chunks => _chunks;

    public Region(IWorldMap worldMap, int x, int z, IServiceProvider serviceProvider)
    {
        WorldMap = worldMap;
        X = x;
        Z = z;
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _chunks = Enumerable.Repeat(default(IChunk), ChunkAmount * ChunkAmount).ToArray();
    }

    public IChunk AddChunk(int x, int z)
    {
        if (ContainsChunk(x, z))
        {
            throw new InvalidOperationException($"Failed to add chunk at {x}/{z}. Chunk already exists.");
        }

        var chunk = new Chunk(this, x, z, _serviceProvider);

        _chunks[GetChunkIndex(x, z)] = chunk;

        return chunk;
    }

    public bool ContainsChunk(int x, int z) => GetChunk(x, z) is not null;

    public IChunk GetChunk(int x, int z) => _chunks[GetChunkIndex(x, z)];

    public IBlock GetBlock(int x, int y, int z)
    {
        var chunkX = x / Chunk.Size;
        var chunkZ = z / Chunk.Size;
        IChunk chunk = GetChunk(chunkX, chunkZ);

        if (chunk is null)
        {
            throw new InvalidOperationException($"Cannot find chunk at position: X={chunkX};Z={chunkZ}");
        }

        return chunk.GetBlock(x % Chunk.Size, y, z % Chunk.Size);
    }

    public IBlock SetBlock(BlockType blockType, int x, int y, int z)
    {
        var chunkX = x / Chunk.Size;
        var chunkZ = z / Chunk.Size;
        IChunk chunk = GetChunk(chunkX, chunkZ);

        if (chunk is null)
        {
            throw new InvalidOperationException($"Cannot find chunk at position: X={chunkX};Z={chunkZ}");
        }

        return chunk.SetBlock(blockType, x % Chunk.Size, y, z % Chunk.Size);
    }

    private static int GetChunkIndex(int x, int z) => z + (ChunkAmount * x);
}
