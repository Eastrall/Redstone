using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Registry;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Serialization;
using Redstone.Common.Structures.Biomes;
using Redstone.Common.Structures.Dimensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Redstone.Server.Registry
{
    [Injectable(ServiceLifetime.Singleton)]
    public class Registry : IRegistry
    {
        public const string DimensionRegistryPath = "/opt/redstone/data/dimensions.json";
        public const string BiomesRegistryPath = "/opt/redstone/data/biomes.json";

        private readonly ILogger<Registry> _logger;
        private bool _isLoaded;

        public IEnumerable<Dimension> Dimensions { get; private set; }

        public IEnumerable<Biome> Biomes { get; private set; }

        public Registry(ILogger<Registry> logger)
        {
            _logger = logger;
        }

        public void Load()
        {
            if (_isLoaded)
            {
                throw new InvalidOperationException($"Cannot reload the registry.");
            }

            _logger.LogInformation("Loading regsitry...");

            try
            {
                LoadDimensions();
                LoadBiomes();

                _isLoaded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load registry.");
            }
        }

        private void LoadDimensions()
        {
            Dimensions = JsonSerializer.Deserialize<IEnumerable<Dimension>>(File.ReadAllText(DimensionRegistryPath));
            _logger.LogInformation($"{Dimensions.Count()} dimensions loaded!");
        }

        private void LoadBiomes()
        {
            Biomes = JsonSerializer.Deserialize<IEnumerable<Biome>>(File.ReadAllText(BiomesRegistryPath));
            _logger.LogInformation($"{Biomes.Count()} biomes loaded!");
        }
    }
}
