using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Cryptography;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Handlers.Internal;
using Redstone.Protocol.Handlers.Internal.Transformers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        /// <param name="handlersAssemblies">
        /// Optionnal assemblies to look for handlers. 
        /// If null, use the <see cref="Assembly.GetExecutingAssembly"/> method.
        /// </param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddMinecraftProtocol(this IServiceCollection services, params Assembly[] handlersAssemblies)
        {
            services.AddSingleton<IMinecraftPacketEncryption, MinecraftPacketEncryption>();

            AddHandlerSystem(services, handlersAssemblies);

            return services;
        }

        /// <summary>
        /// Adds all services related to the packet handler system.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="assemblies">
        /// Optionnal assemblies to look for handlers. 
        /// If null, use the <see cref="Assembly.GetExecutingAssembly"/> method.
        /// </param>
        private static void AddHandlerSystem(IServiceCollection services, IEnumerable<Assembly> assemblies = null)
        {
            IDictionary<MinecraftUserStatus, IHandlerActionCache> invokerCache = PacketHandlerLoader.LoadHandlers(assemblies ?? new[] { Assembly.GetExecutingAssembly() });
            
            services.AddSingleton<IPacketHandler, PacketHandlerInvoker>(serviceProvider =>
            {
                var cache = invokerCache.ToDictionary(x => x.Key, x => ActivatorUtilities.CreateInstance<HandlerActionInvokerCache>(serviceProvider, x.Value));
                return new PacketHandlerInvoker(cache);
            }); 
            //services.TryAddSingleton<HandlerActionInvokerCache>();
            services.TryAddSingleton<IHandlerFactory, HandlerFactory>();

            services.TryAddSingleton<ParameterTransformerCache>();
            services.TryAddSingleton<IParameterFactory, ParameterFactory>();
            services.TryAddSingleton<IParameterTransformer, ParameterTransformer>();

            services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();
        }
    }
}
