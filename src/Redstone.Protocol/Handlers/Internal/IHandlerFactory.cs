using System;

namespace Redstone.Protocol.Handlers.Internal
{
    /// <summary>
    /// Provides methods to create and release handlers.
    /// </summary>
    internal interface IHandlerFactory
    {
        /// <summary>
        /// Creates a new handler instance.
        /// </summary>
        /// <param name="handlerType">Handler type.</param>
        /// <returns>New handler instance.</returns>
        object CreateHandler(Type handlerType);

        /// <summary>
        /// Releases an handler resources if it implements the <see cref="IDisposable"/> interface.
        /// </summary>
        /// <param name="handler">Handler instance.</param>
        void ReleaseHandler(object handler);
    }
}
