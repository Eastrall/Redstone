using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Extensions;
using Redstone.Common.IO;
using System;
using System.IO;

namespace Redstone.Server.World
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class WorldManager : IWorldManager
    {
        public static readonly string MapPath = Path.Combine(EnvironmentExtensions.GetCurrentEnvironementDirectory(), "data", "world");
        private readonly IServiceProvider _serviceProvider;
        private bool _isLoaded;

        public string Name { get; private set; }

        public IWorldMap Overworld { get; private set; }

        public IWorldMap Nether { get; private set; }

        public IWorldMap End { get; private set; }

        public WorldManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Load(string worldName)
        {
            if (_isLoaded)
            {
                throw new InvalidOperationException("Maps have been already loded.");
            }

            string worldPath = Path.Combine(MapPath, worldName);

            DirectoryUtilities.CreateDirectoryIfNotExist(MapPath);
            DirectoryUtilities.CreateDirectoryIfNotExist(worldPath);

            Overworld = new WorldMap("minecraft:overworld", _serviceProvider);
            Overworld.StartUpdate();

            // DEBUG
            IRegion region = Overworld.AddRegion(0, 0);
            IChunk chunk = region.AddChunk(0, 0);

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    chunk.SetBlock(BlockType.Dirt, x, 1, z);
                }
            }

            _isLoaded = true;
            Name = worldName;
        }

        public void Save()
        {
            if (!_isLoaded)
            {
                throw new InvalidOperationException($"Cannot save '{Name}' because the world is not loaded.");
            }

            // TODO: save maps
        }

        public void Dispose()
        {
            Save();
            StopAndDisposeMap(Overworld);
            StopAndDisposeMap(Nether);
            StopAndDisposeMap(End);
        }

        private static void StopAndDisposeMap(IWorldMap map)
        {
            map.StopUpdate();
            map.Dispose();
        }
    }
}
