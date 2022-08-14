using System;

namespace Redstone.Protocol.Handlers.Exceptions;

/// <summary>
/// This exception is thrown when a handler action has not been found.
/// </summary>
public class HandlerActionNotFoundException : Exception
{
    /// <summary>
    /// Gets the handler action.
    /// </summary>
    public object HandlerAction { get; }

    /// <summary>
    /// Creates and initializes a new <see cref="HandlerActionNotFoundException"/>.
    /// </summary>
    /// <param name="handlerAction"></param>
    public HandlerActionNotFoundException(object handlerAction)
        : base($"Cannot find handler : '{handlerAction}'.")
    {
        HandlerAction = handlerAction;
    }
}
