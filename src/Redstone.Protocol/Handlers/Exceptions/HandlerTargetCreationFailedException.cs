using System;

namespace Redstone.Protocol.Handlers.Exceptions
{
    /// <summary>
    /// This exception is thrown when the handler target creation has failed.
    /// </summary>
    public class HandlerTargetCreationFailedException : Exception
    {
        /// <summary>
        /// Gets the handler type.
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// Creates and initializes a new <see cref="HandlerTargetCreationFailedException"/> instance.
        /// </summary>
        /// <param name="handlerType"></param>
        public HandlerTargetCreationFailedException(Type handlerType)
        {
            HandlerType = handlerType;
        }
    }
}
