using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteMessageHandler.Extensions;

internal static class ReflectionExtensions
{
    /// <summary>
    /// Check if the given source type implements the given interface type.
    /// </summary>
    /// <param name="sourceType">Source type.</param>
    /// <param name="interfaceType">Interface type.</param>
    /// <returns>True if the source type implements the given generic type.</returns>
    /// <exception cref="ArgumentNullException">Source type or given interface type is null.</exception>
    /// <exception cref="ArgumentException">Given interface type is not an interface.</exception>
    public static bool ImplementsInterface(this Type sourceType, Type interfaceType)
    {
        if (sourceType is null)
        {
            throw new ArgumentNullException(nameof(sourceType));
        }

        if (interfaceType is null)
        {
            throw new ArgumentNullException(nameof(interfaceType));
        }

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"The given interface type '{interfaceType.FullName}' is not an interface.");
        }

        if (interfaceType.IsGenericType)
        {
            return sourceType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }
        else
        {
            return sourceType.GetInterfaces().Any(x => x == interfaceType);
        }
    }

    /// <summary>
    /// Gets the parameters types of a generic interface type implemented by the given source type.
    /// </summary>
    /// <param name="sourceType">Source type.</param>
    /// <param name="interfaceType">Interface type.</param>
    /// <returns>The generic interface parameters types.</returns>
    /// <exception cref="ArgumentNullException">Source type or given interface type is null.</exception>
    /// <exception cref="ArgumentException">Given interface type is not an interface.</exception>
    public static IEnumerable<Type>? GetGenericInterfaceParameters(this Type sourceType, Type interfaceType)
    {
        if (sourceType is null)
        {
            throw new ArgumentNullException(nameof(sourceType));
        }

        if (interfaceType is null)
        {
            throw new ArgumentNullException(nameof(interfaceType));
        }

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"The given interface type '{interfaceType.FullName}' is not an interface.");
        }

        if (interfaceType.IsGenericType)
        {
            return sourceType.GetInterfaces().SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType)?.GetGenericArguments();
        }
        else
        {
            return sourceType.GetInterfaces().SingleOrDefault(x => x == interfaceType)?.GetGenericArguments();
        }
    }
}
