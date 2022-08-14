using LiteMessageHandler.Extensions;
using LiteMessageHandler.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteMessageHandler;

public class MessageHandlerDispatcher : IMessageHandlerDispatcher
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly MessageHandlerActionCache _handlerCache;

    public MessageHandlerDispatcher(IServiceProvider? serviceProvider = null, params Assembly[] assemblies)
    {
        IEnumerable<Assembly> assembliesToLoad = assemblies.Length > 0 ? assemblies : AppDomain.CurrentDomain.GetAssemblies();

        _serviceProvider = serviceProvider;

        Dictionary<Type, MessageHandlerAction>? handlers = assembliesToLoad.SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.ImplementsInterface(typeof(IMessageHandler<>)))
            .Select(x => new MessageHandlerAction(x))
            .ToDictionary(keySelector: x => x.HandlerParameterType, elementSelector: x => x);

        _handlerCache = new MessageHandlerActionCache(handlers);
    }

    public MessageHandler? GetHandler(Type? handlerType)
    {
        if (handlerType == null)
        {
            throw new ArgumentNullException(nameof(handlerType));
        }

        MessageHandlerAction? handler = _handlerCache.GetMessageHandler(handlerType);

        if (handler == null)
        {
            return null;
        }

        return new MessageHandler(handler.CreateInstance(_serviceProvider), handler);
    }

    public MessageHandler? GetHandler<TMessage>() where TMessage : class
    {
        return GetHandler(typeof(TMessage));
    }

    public void Dispatch<TMessage>(TMessage message) where TMessage : class
    {
        MessageHandler? handler = GetHandler(typeof(TMessage));

        if (handler == null)
        {
            throw new InvalidOperationException($"Cannot find handler for type '{typeof(TMessage).FullName}'.");
        }

        handler.Execute(message!);
    }

    public void Dispose()
    {
        _handlerCache?.Dispose();
    }
}
