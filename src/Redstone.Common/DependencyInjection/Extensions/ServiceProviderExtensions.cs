using Microsoft.Extensions.DependencyInjection;
using System;

namespace Redstone.Common.DependencyInjection.Extensions
{
    /// <summary>
    /// Provides extensions for the service provider.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Creates a new instance of <typeparamref name="TInstance"/> using the service provider to inject dependencies.
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static TInstance CreateInstance<TInstance>(this IServiceProvider serviceProvider, params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<TInstance>(serviceProvider, parameters);
        }
    }
}
