using System;

namespace Redstone.Protocol.Handlers.Internal;

internal class HandlerActionInvokerCacheEntry
{
    /// <summary>
    /// Gets the target handler type.
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// Gets the handler factory creator used to create new handler instances.
    /// </summary>
    public Func<Type, object> HandlerFactory { get; }

    /// <summary>
    /// Gets the handler releaser used to release handler's resources.
    /// </summary>
    public Action<object> HandlerReleaser { get; }

    /// <summary>
    /// Gets the handler action executor.
    /// </summary>
    public HandlerExecutor HandlerExecutor { get; }

    /// <summary>
    /// Creates a new <see cref="HandlerActionInvokerCacheEntry"/> instance.
    /// </summary>
    /// <param name="handlerType">Target handler type.</param>
    /// <param name="handlerFactory">Handler factory creator function.</param>
    /// <param name="handlerReleaser">Handler releaser action.</param>
    /// <param name="handlerExecutor">Handler action executor.</param>
    internal HandlerActionInvokerCacheEntry(Type handlerType, Func<Type, object> handlerFactory, Action<object> handlerReleaser, HandlerExecutor handlerExecutor)
    {
        HandlerType = handlerType;
        HandlerFactory = handlerFactory;
        HandlerReleaser = handlerReleaser;
        HandlerExecutor = handlerExecutor;
    }
}
