using Redstone.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Redstone.Common.Structures.Blocks
{
    [DebuggerDisplay("{Name}")]
    public class BlockData
    {
        /// <summary>
        /// Gets the block name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the block type.
        /// </summary>
        public BlockType Type { get; }

        /// <summary>
        /// Gets the block properties list.
        /// </summary>
        public IEnumerable<BlockPropertyData> Properties { get; }

        /// <summary>
        /// Gets the block available states.
        /// </summary>
        public IEnumerable<BlockStateData> States { get; }

        /// <summary>
        /// Creates a new <see cref="BlockData"/> instance.
        /// </summary>
        /// <param name="name">Block name.</param>
        /// <param name="properties">Block properties.</param>
        /// <param name="blockStates">Block available states.</param>
        public BlockData(string name, IEnumerable<BlockPropertyData> properties, IEnumerable<BlockStateData> blockStates)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Block data name cannot be null or empty.");
            }

            Name = name;
            Properties = properties ?? Enumerable.Empty<BlockPropertyData>();
            States = blockStates ?? Enumerable.Empty<BlockStateData>();

            if (!Enum.TryParse(name.Replace("minecraft:", "").Trim().ToPascalCase(), out BlockType type))
            {
                throw new InvalidOperationException($"Failed to convert '{name}' into 'BlockType' enum.");
            }

            Type = type;
        }
    }
}
