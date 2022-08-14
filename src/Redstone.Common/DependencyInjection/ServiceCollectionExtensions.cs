using Microsoft.Extensions.DependencyInjection;
using Redstone.Common.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace Redstone.Common.DependencyInjection;

/// <summary>
/// Extensions for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services tagged with a <see cref="InjectableAttribute"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assemblies">Assemblies to look for injectable services.</param>
    public static void AddInjectableServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        var injectableServicesTypes = ReflectionHelper.GetClassesWithCustomAttribute<InjectableAttribute>(assemblies);

        foreach (var serviceType in injectableServicesTypes)
        {
            InjectableAttribute serviceAttribute = GetTypeAttribute<InjectableAttribute>(serviceType);
            Type[] serviceInterfacesTypes = serviceType.GetInterfaces();
            ServiceLifetime serviceLifeTime = serviceAttribute != null ? serviceAttribute.LifeTime : ServiceLifetime.Transient;

            if (serviceInterfacesTypes.Any())
            {
                foreach (var serviceInterface in serviceInterfacesTypes)
                {
                    services.Add(new ServiceDescriptor(serviceInterface, serviceType, serviceLifeTime));
                }
            }
            else
            {
                services.Add(new ServiceDescriptor(serviceType, serviceLifeTime));
            }
        }
    }

    /// <summary>
    /// Gets an <typeparamref name="TAttribute"/> attribute metadata from a given type.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type.</typeparam>
    /// <param name="type">Type.</param>
    /// <returns></returns>
    private static TAttribute GetTypeAttribute<TAttribute>(Type type) where TAttribute : Attribute
    {
        return type.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(TAttribute)) as TAttribute;
    }
}
