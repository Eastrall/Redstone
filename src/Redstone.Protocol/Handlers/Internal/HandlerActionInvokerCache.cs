using Redstone.Protocol.Handlers.Internal;
using Redstone.Protocol.Handlers.Internal.Extensions;
using Redstone.Protocol.Handlers.Internal.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Redstone.Protocol.Handlers.Internal;

internal sealed class HandlerActionInvokerCache : IDisposable
{
    private static readonly object _syncLock = new();
    private readonly IDictionary<object, HandlerActionInvokerCacheEntry> _cache;
    private readonly IHandlerActionCache _handlerCache;
    private readonly IHandlerFactory _handlerFactory;

    /// <summary>
    /// Creates a new <see cref="HandlerActionInvokerCache"/> instance.
    /// </summary>
    /// <param name="handlerCache">Handler action cache.</param>
    /// <param name="handlerFactory">Handler factory.</param>
    public HandlerActionInvokerCache(IHandlerActionCache handlerCache, IHandlerFactory handlerFactory)
    {
        _cache = new ConcurrentDictionary<object, HandlerActionInvokerCacheEntry>();
        _handlerCache = handlerCache;
        _handlerFactory = handlerFactory;
    }

    /// <summary>
    /// Gets a <see cref="HandlerActionInvokerCacheEntry"/> based on a handler action.
    /// </summary>
    /// <remarks>
    /// <see cref="HandlerActionInvokerCacheEntry"/> contains the handler factory creator 
    /// and the handler action executor.
    /// </remarks>
    /// <param name="handlerAction">Handler action.</param>
    /// <returns>
    /// Existing <see cref="HandlerActionInvokerCacheEntry"/> in cache; 
    /// if the entry doesn't exist, it creates a new <see cref="HandlerActionInvokerCacheEntry"/>, 
    /// caches it and returns it.
    /// </returns>
    public HandlerActionInvokerCacheEntry GetCachedHandlerAction(object handlerAction)
    {
        if (!_cache.TryGetValue(handlerAction, out HandlerActionInvokerCacheEntry cacheEntry))
        {
            HandlerActionModel handlerActionModel = _handlerCache.GetHandlerAction(handlerAction);

            if (handlerActionModel is null)
            {
                return null;
            }

            object[] defaultHandlerActionParameters = handlerActionModel.Method.GetMethodParametersDefaultValues();

            cacheEntry = new HandlerActionInvokerCacheEntry(
                handlerActionModel.HandlerTypeInfo.AsType(),
                _handlerFactory.CreateHandler,
                _handlerFactory.ReleaseHandler,
                new HandlerExecutor(handlerActionModel.HandlerTypeInfo, handlerActionModel.Method, defaultHandlerActionParameters));

            if (!_cache.ContainsKey(handlerAction))
            {
                lock (_syncLock)
                {
                    if (!_cache.ContainsKey(handlerAction))
                    {
                        _cache.TryAdd(handlerAction, cacheEntry);
                    }
                    else
                    {
                        _cache.TryGetValue(handlerAction, out cacheEntry);
                    }
                }
            }
        }

        return cacheEntry;
    }

    /// <summary>
    /// Dispose the <see cref="HandlerActionInvokerCache"/> resources.
    /// </summary>
    public void Dispose()
    {
        _cache.Clear();
    }
}
