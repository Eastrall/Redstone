using System.Collections.Generic;
using System.Diagnostics;

namespace Redstone.Common.Structures.Blocks;

[DebuggerDisplay("BlockState {Id} | Default: {IsDefault}")]
public class BlockStateData
{
    public int Id { get; }

    public bool IsDefault { get; }

    public IReadOnlyDictionary<string, string> Properties { get; }

    public BlockStateData(int id, bool isDefault, IDictionary<string, string> properties)
    {
        Id = id;
        IsDefault = isDefault;
        Properties = new Dictionary<string, string>(properties);
    }
}
