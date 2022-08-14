using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redstone.Common.Reflection;

/// <summary>
/// Provides helper methods for reflection.
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// Get classes with a custom attribute.
    /// </summary>
    /// <param name="attributeType">Attribute type</param>
    /// <param name="assemblies">Assemblies to reflect.</param>
    /// <returns></returns>
    public static IEnumerable<TypeInfo> GetClassesWithCustomAttribute(Type attributeType, params Assembly[] assemblies)
    {
        return from x in assemblies.SelectMany(x => x.GetTypes())
               where x.GetCustomAttribute(attributeType) != null
               select x.GetTypeInfo();
    }

    /// <summary>
    /// Get classes with a custom attribute.
    /// </summary>
    /// <typeparam name="T">Attribute type</typeparam>
    /// <param name="assemblies">Assemblies to reflect.</param>
    /// <returns></returns>
    public static IEnumerable<TypeInfo> GetClassesWithCustomAttribute<T>(params Assembly[] assemblies) 
        => GetClassesWithCustomAttribute(typeof(T), assemblies);

    /// <summary>
    /// Get classes that are assignable from a given type. (inherits from)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="assemblies">Assemblies to reflect.</param>
    /// <returns></returns>
    public static IEnumerable<TypeInfo> GetClassesAssignableFrom(Type type, params Assembly[] assemblies)
    {
        return from x in assemblies.SelectMany(x => x.GetTypes())
               where x.GetInterfaces().Any(i => i.IsAssignableFrom(type))
               select x.GetTypeInfo();
    }

    /// <summary>
    /// Get classes that are assignable from a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assemblies">Assemblies to reflect.</param>
    /// <returns></returns>
    public static IEnumerable<TypeInfo> GetClassesAssignableFrom<T>(params Assembly[] assemblies) 
        => GetClassesAssignableFrom(typeof(T), assemblies);

    /// <summary>
    /// Get methods with custom attributes.
    /// </summary>
    /// <param name="attributeType">Attribute type</param>
    /// <param name="assemblies">Assemblies to reflect.</param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethodsWithAttributes(Type attributeType, params Assembly[] assemblies)
    {
        return from x in assemblies.SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMethods())
               where x.GetCustomAttributes(attributeType)?.Count() > 0
               select x;
    }

    /// <summary>
    /// Get methods with custom attributes.
    /// </summary>
    /// <typeparam name="T">Attribute type</typeparam>
    /// <param name="assemblies">Assemblies to reflect.</param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethodsWithAttributes<T>(params Assembly[] assemblies) 
        => GetMethodsWithAttributes(typeof(T), assemblies);
}
