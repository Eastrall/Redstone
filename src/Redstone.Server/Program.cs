using LiteNetwork.Server.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Redstone.Configuration.Yaml;
using System;
using System.Threading.Tasks;

namespace Redstone.Server
{
    class Program
    {
        static Task Main()
        {
            IHost serverHost = new HostBuilder()
                .ConfigureAppConfiguration((host, builder) =>
                {
                    builder.SetBasePath("/opt/redstone/config");
                    builder.AddYamlFile("server.yml");
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
                })
                .UseConsoleLifetime()
                .Build();

            return serverHost.RunAsync();
        }
    }
}
