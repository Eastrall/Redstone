using Microsoft.Extensions.DependencyInjection;
using Redstone.Protocol.Abstractions;
using Redstone.Protocol.Cryptography;

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

            return services;
        }
    }
}
