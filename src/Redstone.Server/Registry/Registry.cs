using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Registry;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Serialization;
using Redstone.Common.Structures.Biomes;
using Redstone.Common.Structures.Blocks;
using Redstone.Common.Structures.Dimensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Redstone.Server.Registry
{
    [Injectable(ServiceLifetime.Singleton)]
    public class Registry : IRegistry
    {
        public const string DimensionRegistryPath = "/opt/redstone/data/dimensions.json";
        public const string BiomesRegistryPath = "/opt/redstone/data/biomes.json";
        public const string BlocksDataPath = "/opt/redstone/data/blocks.json";

        private readonly ILogger<Registry> _logger;
        private bool _isLoaded;

        public IEnumerable<Dimension> Dimensions { get; private set; }

        public IEnumerable<Biome> Biomes { get; private set; }

        public IEnumerable<BlockData> Blocks { get; private set; }

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
                LoadBlocksData();

                _isLoaded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load registry.");
            }
        }

        private void LoadDimensions()
        {
            Dimensions = Common.Serialization.JsonSerializer.Deserialize<IEnumerable<Dimension>>(File.ReadAllText(DimensionRegistryPath));
            _logger.LogInformation($"{Dimensions.Count()} dimensions loaded!");
        }

        private void LoadBiomes()
        {
            Biomes = Common.Serialization.JsonSerializer.Deserialize<IEnumerable<Biome>>(File.ReadAllText(BiomesRegistryPath));
            _logger.LogInformation($"{Biomes.Count()} biomes loaded!");
        }

        private void LoadBlocksData()
        {
            using var blocksFile = File.OpenRead(BlocksDataPath);
            JsonDocument blocksDocument = JsonDocument.Parse(blocksFile);

            var blocks = new List<BlockData>();

            foreach (JsonProperty blockElement in blocksDocument.RootElement.EnumerateObject())
            {
                string blockName = blockElement.Name;
                var blockProperties = Enumerable.Empty<BlockPropertyData>();
                var blockStates = Enumerable.Empty<BlockStateData>();

                try
                {
                    foreach (JsonProperty element in blockElement.Value.EnumerateObject())
                    {
                        if (element.Name == "properties")
                        {
                            blockProperties = element.Value
                                .EnumerateObject()
                                .Select(x => new BlockPropertyData(x.Name, x.Value.EnumerateArray().Select(p => p.GetString()).ToList()))
                                .ToList() ?? Enumerable.Empty<BlockPropertyData>();
                        }
                        else if (element.Name == "states")
                        {
                            blockStates = element.Value.EnumerateArray().Select(x =>
                            {
                                var elementObjectProperties = x.EnumerateObject();
                                int id = elementObjectProperties.FirstOrDefault(p => p.Name == "id").Value.GetInt32();

                                bool isDefault = false;
                                if (elementObjectProperties.Any(p => p.Name == "default"))
                                {
                                    isDefault = elementObjectProperties.FirstOrDefault(p => p.Name == "default").Value.GetBoolean();
                                }

                                var properties = new Dictionary<string, string>(Enumerable.Empty<KeyValuePair<string, string>>());
                                if (elementObjectProperties.Any(p => p.Name == "properties"))
                                {
                                    properties = x.EnumerateObject()
                                       .FirstOrDefault(p => p.Name == "properties").Value
                                       .EnumerateObject()
                                           .ToDictionary(p => p.Name, p => p.Value.GetString());
                                }

                                return new BlockStateData(id, isDefault, properties);
                            }).ToList();
                        }
                    }

                    var block = new BlockData(blockName, blockProperties, blockStates);

                    blocks.Add(block);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"Failed to load block data '{blockName}'.");
                }
            }

            Blocks = blocks;
            _logger.LogInformation($"{Blocks.Count()} blocks loaded!");
        }
    }
}
