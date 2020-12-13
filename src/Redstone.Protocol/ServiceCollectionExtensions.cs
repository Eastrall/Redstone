using Microsoft.Extensions.DependencyInjection;
using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Cryptography;
using Redstone.Protocol.Handlers;

namespace Redstone.Protocol
{
    /// <summary>
    /// Provides extensions to the <see cref="IServiceCollection"/> to add protocol related services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Minecraft protocol related stuff.
        /// </summary>
        /// <param name="services">Servie collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddMinecraftProtocol(this IServiceCollection services)
        {
            services.AddSingleton<IMinecraftPacketEncryption, MinecraftPacketEncryption>();

            AddHandlerSystem(services);

            return services;
        }

        /// <summary>
        /// Adds all services related to the packet handler system.
        /// </summary>
        /// <param name="services">Service collection.</param>
        private static void AddHandlerSystem(IServiceCollection services)
        {
            services.AddSingleton<IPacketHandler, PacketHandler>();
        }
    }
}
