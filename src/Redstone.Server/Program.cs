using LiteNetwork.Common;
using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;
using Redstone.Abstractions;
using Redstone.Common.Configuration;
using Redstone.Common.DependencyInjection;
using Redstone.Common.Extensions;
using Redstone.Protocol;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Redstone.Server
{
    class Program
    {
        static Task Main()
        {
            IHost serverHost = new HostBuilder()
                .ConfigureAppConfiguration((host, config) =>
                {
                    config.SetBasePath(EnvironmentExtensions.GetCurrentEnvironementDirectory());
                    config.AddYamlFile("config/server.yml", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddFilter("LiteNetwork", LogLevel.Warning);
                    builder.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.Configure<ServerOptions>(context.Configuration.GetSection("server"));
                    services.Configure<GameOptions>(context.Configuration.GetSection("game"));
                    services.AddInjectableServices(Assembly.GetExecutingAssembly());
                    services.AddMinecraftProtocol(Assembly.GetExecutingAssembly());
                })
                .UseLiteServer<IRedstoneServer, RedstoneServer, MinecraftUser>((context, options) =>
                {
                    var serverConfiguration = context.Configuration.GetSection("server").Get<ServerOptions>();

                    if (serverConfiguration is null)
                    {
                        throw new InvalidProgramException($"Failed to load server settings.");
                    }

                    options.Host = serverConfiguration.Ip;
                    options.Port = serverConfiguration.Port;
                    options.PacketProcessor = new MinecraftPacketProcessor();
                    options.ReceiveStrategy = ReceiveStrategyType.Queued;
                })
                .UseNLog()
                .UseConsoleLifetime()
                .Build();

            return serverHost.RunAsync();
        }
    }
}
