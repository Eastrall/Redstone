using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Redstone.Abstractions.World;
using Redstone.Server.World;
using System;

namespace Redstone.Server.Tests.Mocks
{
    public class WorldMapMock : Mock<IWorldMap>
    {
        public static IWorldMap Create(string name)
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton(new Mock<ILogger<WorldMap>>().Object)
                .AddSingleton(s => new BlockFactoryMock().Object)
                .BuildServiceProvider();
            var map = new WorldMap(name, serviceProvider);

            return map;
        }
    }
}
