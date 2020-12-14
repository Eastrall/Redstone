using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Transformers
{
    /// <summary>
    /// Provides methods to create a parameter dynamicaly.
    /// </summary>
    internal interface IParameterFactory
    {
        /// <summary>
        /// Creates a new parameter instance based on the given type information.
        /// </summary>
        /// <param name="type">Type information.</param>
        /// <returns>New instance as <see cref="object"/>.</returns>
        object Create(TypeInfo type);
    }
}
