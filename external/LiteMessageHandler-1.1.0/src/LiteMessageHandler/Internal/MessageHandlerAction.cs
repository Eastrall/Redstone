using LiteMessageHandler.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteMessageHandler.Internal;

internal class MessageHandlerAction
{
    private readonly ObjectFactory _objectFactory;

    public Type HandlerType { get; }

    public Type HandlerParameterType { get; }

    public MessageHandlerExecutor Executor { get; }

    public IEnumerable<Attribute> Attributes { get; }

    public MessageHandlerAction(Type? handlerType)
    {
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        HandlerParameterType = handlerType.GetGenericInterfaceParameters(typeof(IMessageHandler<>))?.Single()
            ?? throw new ArgumentException("Cannot find generic parameter type.");
        Executor = new MessageHandlerExecutor(HandlerType, HandlerParameterType);
        _objectFactory = ActivatorUtilities.CreateFactory(handlerType, Type.EmptyTypes);
        Attributes = handlerType.GetCustomAttributes(false).Cast<Attribute>().AsEnumerable();
    }

    public object? CreateInstance(IServiceProvider? serviceProvider)
    {
        if (serviceProvider is null)
        {
            return Activator.CreateInstance(HandlerType);
        }

        using var scope = serviceProvider.CreateScope();

        return _objectFactory(scope.ServiceProvider, null);
    }
}
