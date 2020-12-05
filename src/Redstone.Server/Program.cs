using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Redstone.Common.Configuration;
using Redstone.Protocol;
using System;
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
                    config.SetBasePath("/opt/redstone/config");
                    config.AddYamlFile("server.yml", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.Configure<ServerConfiguration>(context.Configuration.GetSection("server"));
                    services.Configure<GameConfiguration>(context.Configuration.GetSection("game"));
                    services.AddMinecraftProtocol();
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddFilter("LiteNetwork", LogLevel.Information);

                    // Debug stuff: make a configuration for this
                    builder.SetMinimumLevel(LogLevel.Trace);
                })
                .UseLiteServer<RedstoneServer, MinecraftUser>((context, options) =>
                {
                    var serverConfiguration = context.Configuration.GetSection("server").Get<ServerConfiguration>();

                    if (serverConfiguration is null)
                    {
                        throw new InvalidProgramException($"Failed to load server settings.");
                    }

                    options.Host = serverConfiguration.Ip;
                    options.Port = serverConfiguration.Port;
                    options.PacketProcessor = new MinecraftPacketProcessor();
                })
                .UseConsoleLifetime()
                .Build();

            return serverHost.RunAsync();
        }
    }
}
