using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.Registry;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.Structures.Blocks;
using Redstone.Server.Registry;
using Redstone.Server.World;
using System;
using System.Collections.Generic;

namespace Redstone.Server.Tests.Mocks;

public class WorldMapMock : Mock<IWorldMap>
{
    public static IWorldMap Create(string name)
    {
        IServiceProvider serviceProvider = new ServiceCollection()
            .AddSingleton(new Mock<ILogger<WorldMap>>().Object)
            .AddSingleton<IBlockFactory>(s =>
            {
                IRegistry registry = s.GetRequiredService<IRegistry>();
                var blockFactoryMock = new Mock<IBlockFactory>();
                blockFactoryMock.Setup(x => x.CreateBlock(It.IsAny<BlockType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IChunk>()))
                             .Returns<BlockType, int, int, int, IChunk>((type, x, y, z, chunk) =>
                             {
                                 return new Block(x, y, z, chunk, new BlockData(type.ToString(), (int)type, null, new[]
                                 {
                                     new BlockStateData((int)type, true, new Dictionary<string, string>())
                                 }), registry);
                             });

                return blockFactoryMock.Object;
            })
            .AddSingleton<IRegistry>(s =>
            {
                var registry = new DataRegistry(new Mock<ILogger<DataRegistry>>().Object);
                registry.Load();

                return registry;
            })
            .BuildServiceProvider();
        var map = new WorldMap(name, serviceProvider);

        return map;
    }
}
