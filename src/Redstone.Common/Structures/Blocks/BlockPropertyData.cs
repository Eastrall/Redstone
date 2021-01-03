using System.Collections.Generic;
using System.Diagnostics;

namespace Redstone.Common.Structures.Blocks
{
    [DebuggerDisplay("{Name}")]
    public class BlockPropertyData
    {
        public string Name { get; }

        public IEnumerable<string> Values { get; }

        public BlockPropertyData(string name, IEnumerable<string> values)
        {
            Name = name;
            Values = values;
        }
    }
}
