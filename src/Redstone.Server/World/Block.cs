using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Structures.Blocks;
using System;
using System.Diagnostics;
using System.Linq;

namespace Redstone.Server.World;

[DebuggerDisplay("{Type}: {X}/{Y}/{Z} (Chunk: {Chunk.X}/{Chunk.Z}")]
public sealed class Block : IBlock
{
    private readonly IRegistry _registry;
    private BlockData _blockData;

    public bool IsAir => Type is BlockType.Air or BlockType.CaveAir or BlockType.VoidAir;

    public bool IsFluid => Type is BlockType.Water or BlockType.Lava;

    public bool IsSolid => !IsAir && !IsFluid;

    public int Id => State.Id;

    public BlockType Type => _blockData.Type;

    public int X { get; }

    public int Y { get; }

    public int Z { get; }

    public IChunk Chunk { get; }

    public BlockStateData State { get; private set; }

    internal Block(int x, int y, int z, IChunk chunk, BlockData blockData, IRegistry registry)
    {
        X = x;
        Y = y;
        Z = z;
        Chunk = chunk ?? throw new ArgumentNullException(nameof(chunk));
        _blockData = blockData ?? throw new ArgumentNullException(nameof(blockData), "Cannot create a block with no block data.");
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        State = _blockData.States.SingleOrDefault(x => x.IsDefault);

        if (State is null)
        {
            throw new InvalidOperationException($"Failed to initialize block. Cannot find default state for block data: {_blockData.Name}");
        }
    }

    public void SetType(BlockType type)
    {
        if (type != Type)
        {
            _blockData = _registry.Blocks.FirstOrDefault(x => x.Type == type) ??
                throw new ArgumentException($"Failed to find block type: {type} in registry.");
            SetState(_blockData.States.Single(x => x.IsDefault));
        }
    }

    public void SetState(BlockStateData state)
    {
        if (state is null)
        {
            throw new InvalidOperationException($"Cannot assign a null state to current block.");
        }

        if (state.Id != State.Id)
        {
            State = state;
        }
    }

    public IBlock GetRelative(BlockFaceType blockFace)
    {
        int x = blockFace switch
        {
            BlockFaceType.North => X + 1,
            BlockFaceType.South => X - 1,
            _ => X
        };
        int y = blockFace switch
        {
            BlockFaceType.Top => Y + 1,
            BlockFaceType.Bottom => Y - 1,
            _ => Y
        };
        int z = blockFace switch
        {
            BlockFaceType.East => Z + 1,
            BlockFaceType.West => Z - 1,
            _ => Z
        };

        return Chunk.Region.WorldMap.GetBlock(x, y, z);
    }
}
