using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common.Extensions;
using Redstone.Common.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Redstone.Server.World.Palettes;

internal class BlockStatePalette : IPalette
{
    public event EventHandler<int> Resized;

    private readonly int _maximumBitsPerBlock;
    private readonly IDictionary<int, int> _blockStatesCount;

    private int _bitsPerBlock;
    private int?[] _blockStates;

    public bool IsFull => _blockStates.All(x => x.HasValue);

    public bool IsUsingGlobalPalette => _bitsPerBlock == _maximumBitsPerBlock;

    public int BitsPerBlock => _bitsPerBlock;

    public int Count => _blockStates.Count(x => x.HasValue);

    public BlockStatePalette(byte bitCount, int maximumBitsPerBlock)
    {
        _bitsPerBlock = bitCount;
        _maximumBitsPerBlock = maximumBitsPerBlock;
        _blockStates = new int?[1 << _bitsPerBlock];
        _blockStatesCount = new Dictionary<int, int>();
    }

    public int GetState(int paletteIndex) => _blockStates[paletteIndex].GetValueOrDefault(-1);

    public int SetState(int blockStateId)
    {
        int paletteIndex;

        if (IsUsingGlobalPalette)
        {
            paletteIndex = blockStateId;
        }
        else
        {
            paletteIndex = Array.IndexOf(_blockStates, blockStateId);

            if (paletteIndex == -1) // Block state not in palette.
            {
                // Search for the first empty state slot in array.
                paletteIndex = Array.IndexOf(_blockStates, null);

                if (paletteIndex == -1)
                {
                    // Palette is empty, we need to increase internal block state array.
                    Array.Resize(ref _blockStates, _blockStates.Length + 1);
                    paletteIndex = Array.IndexOf(_blockStates, null);
                }

                int neededBits = paletteIndex.NeededBits();

                if (neededBits > BitsPerBlock)
                {
                    _bitsPerBlock = neededBits;
                    Resized?.Invoke(this, neededBits);
                }

                _blockStates[paletteIndex] = blockStateId;
            }
        }

        return paletteIndex;
    }

    [ExcludeFromCodeCoverage]
    public void Serialize(MinecraftStream stream)
    {
        stream.WriteVarInt32(Count);

        for (var i = 0; i < Count; i++)
        {
            if (_blockStates[i].HasValue)
            {
                stream.WriteVarInt32(_blockStates[i].Value);
            }
        }
    }
}
