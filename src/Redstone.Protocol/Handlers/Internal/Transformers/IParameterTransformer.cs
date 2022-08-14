using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Transformers;

/// <summary>
/// Provides a mechanism to transform a source parameter into a destination parameter.
/// </summary>
internal interface IParameterTransformer
{
    /// <summary>
    /// Transforms a source parameter into a destination parameter using a cached transformer.
    /// </summary>
    /// <param name="originalParameter">Original parameter.</param>
    /// <param name="destinationParameterType">Destination parameter type information.</param>
    /// <returns>Transformed parameter.</returns>
    object Transform(object originalParameter, TypeInfo destinationParameterType);
}
