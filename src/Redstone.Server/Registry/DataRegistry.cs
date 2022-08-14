using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redstone.Abstractions.Registry;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Extensions;
using Redstone.Common.Structures.Biomes;
using Redstone.Common.Structures.Blocks;
using Redstone.Common.Structures.Dimensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Redstone.Server.Registry;

[Injectable(ServiceLifetime.Singleton)]
public class DataRegistry : IRegistry
{
    public const string DimensionRegistryPath = "data/dimensions.json";
    public const string BiomesRegistryPath = "data/biomes.json";
    public const string BlocksDataPath = "data/blocks.json";
    public const string RegistriesDataPath = "data/registries.json";

    private readonly ILogger<DataRegistry> _logger;
    private readonly ConcurrentDictionary<string, int> _items = new();
    private bool _isLoaded;

    public IEnumerable<Dimension> Dimensions { get; private set; }

    public IEnumerable<Biome> Biomes { get; private set; }

    public IEnumerable<BlockData> Blocks { get; private set; }

    public DataRegistry(ILogger<DataRegistry> logger)
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
            LoadItems();
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
        string dimensionsPath = Path.Combine(EnvironmentExtensions.GetCurrentEnvironementDirectory(), DimensionRegistryPath);
        string content = File.ReadAllText(dimensionsPath);

        Dimensions = Common.Serialization.JsonSerializer.Deserialize<IEnumerable<Dimension>>(content);
        _logger.LogInformation($"{Dimensions.Count()} dimensions loaded!");
    }

    private void LoadBiomes()
    {
        string biomesPath = Path.Combine(EnvironmentExtensions.GetCurrentEnvironementDirectory(), BiomesRegistryPath);
        string content = File.ReadAllText(biomesPath);

        Biomes = Common.Serialization.JsonSerializer.Deserialize<IEnumerable<Biome>>(content);
        _logger.LogInformation($"{Biomes.Count()} biomes loaded!");
    }

    private void LoadItems()
    {
        string registriesPath = Path.Combine(EnvironmentExtensions.GetCurrentEnvironementDirectory(), RegistriesDataPath);

        if (!File.Exists(registriesPath))
        {
            throw new FileNotFoundException("Failed to load items from registries file.", registriesPath);
        }

        using var registriesFile = File.OpenRead(registriesPath);
        JsonDocument registriesDocument = JsonDocument.Parse(registriesFile);

        JsonElement itemEntries = registriesDocument.RootElement.GetProperty("minecraft:item").GetProperty("entries");

        foreach (JsonProperty itemEntry in itemEntries.EnumerateObject())
        {
            string name = itemEntry.Name;
            int itemId = itemEntry.Value.GetProperty("protocol_id").GetInt32();

            if (!_items.TryAdd(name, itemId))
            {
                _logger.LogWarning($"Duplicate item: {name}");
            }
        }
    }

    private void LoadBlocksData()
    {
        string blocksPath = Path.Combine(EnvironmentExtensions.GetCurrentEnvironementDirectory(), BlocksDataPath);
        using var blocksFile = File.OpenRead(blocksPath);
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

                if (!_items.TryGetValue(blockName, out int blockItemId))
                {
                    _logger.LogWarning($"Failed to get block item id for block '{blockName}'.");
                    continue;
                }

                var block = new BlockData(blockName, blockItemId, blockProperties, blockStates);

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
