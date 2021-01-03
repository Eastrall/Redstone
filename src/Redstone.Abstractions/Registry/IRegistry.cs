using Redstone.Common.Structures.Biomes;
using Redstone.Common.Structures.Blocks;
using Redstone.Common.Structures.Dimensions;
using System.Collections.Generic;

namespace Redstone.Abstractions.Registry
{
    /// <summary>
    /// Provides a mechanism to load the game registry.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// Gets a collection with all loaded dimensions.
        /// </summary>
        IEnumerable<Dimension> Dimensions { get; }

        /// <summary>
        /// Gets a collection with all loaded biomes.
        /// </summary>
        IEnumerable<Biome> Biomes { get; }

        /// <summary>
        /// Gets a collection with loaded blocks data.
        /// </summary>
        IEnumerable<BlockData> Blocks { get; }

        /// <summary>
        /// Loads the registry resources.
        /// </summary>
        void Load();
    }
}
