using System;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Transformers;

public delegate object TransformerFuntion(object source, object destination);

internal class ParameterTransformerCacheEntry
{
    /// <summary>
    /// Gets the source parameter information.
    /// </summary>
    public TypeInfo Source { get; }

    /// <summary>
    /// Gets the destination parameter information.
    /// </summary>
    public TypeInfo Destination { get; }

    /// <summary>
    /// Gets the method parameter target information type.
    /// </summary>
    public TypeInfo Target { get; }

    /// <summary>
    /// Gets the parameter factory to create new instances of the parameter based on his type information.
    /// </summary>
    public Func<TypeInfo, object> ParameterFactory { get; }

    /// <summary>
    /// Gets the parameter transformer function.
    /// </summary>
    public TransformerFuntion Transformer { get; }

    /// <summary>
    /// Creates a new <see cref="ParameterTransformerCacheEntry"/> instance.
    /// </summary>
    /// <param name="source">Source parameter type information.</param>
    /// <param name="destination">Destination parameter type information.</param>
    /// <param name="target">Method target parameter type information.</param>
    /// <param name="parameterFactory">Parameter factory creator.</param>
    /// <param name="transformer">Parameter tranformer function.</param>
    public ParameterTransformerCacheEntry(TypeInfo source, TypeInfo destination, TypeInfo target, Func<TypeInfo, object> parameterFactory, TransformerFuntion transformer)
    {
        Source = source;
        Destination = destination;
        Target = target;
        ParameterFactory = parameterFactory;
        Transformer = transformer;
    }
}
