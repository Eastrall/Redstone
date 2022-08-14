using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LiteMessageHandler.Internal;

internal class MessageHandlerActionCache : IDisposable
{
    private readonly ConcurrentDictionary<Type, MessageHandlerAction> _cache;

    public MessageHandlerActionCache(IDictionary<Type, MessageHandlerAction> cachedHandlers)
    {
        _cache = new ConcurrentDictionary<Type, MessageHandlerAction>(cachedHandlers);
    }

    public MessageHandlerAction? GetMessageHandler(Type? type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return _cache.TryGetValue(type, out MessageHandlerAction? handler) ? handler : null;
    }

    public void Dispose()
    {
        _cache.Clear();
    }
}
