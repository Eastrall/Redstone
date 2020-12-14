using Redstone.Protocol.Handlers.Internal;
using System;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Transformers
{
    internal class ParameterFactory : IParameterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITypeActivatorCache _typeActivatorCache;

        /// <summary>
        /// Creates a new <see cref="ParameterFactory"/> instance.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="typeActivatorCache">Type Activator cache.</param>
        public ParameterFactory(IServiceProvider serviceProvider, ITypeActivatorCache typeActivatorCache)
        {
            _serviceProvider = serviceProvider;
            _typeActivatorCache = typeActivatorCache;
        }

        /// <inheritdoc />
        public object Create(TypeInfo type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return _typeActivatorCache.Create<object>(_serviceProvider, type.AsType());
        }
    }
}
