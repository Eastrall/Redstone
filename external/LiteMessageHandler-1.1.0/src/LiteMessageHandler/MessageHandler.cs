using LiteMessageHandler.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteMessageHandler;

public class MessageHandler
{
    private readonly MessageHandlerAction _action;

    /// <summary>
    /// Gets the message handler target object.
    /// </summary>
    public object Target { get; }

    internal MessageHandler(object? target, MessageHandlerAction? handlerAction)
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
        _action = handlerAction ?? throw new ArgumentNullException(nameof(handlerAction));
    }

    /// <summary>
    /// Executes the message handler action with the given parameter.
    /// </summary>
    /// <param name="parameter">Handler parameter.</param>
    public void Execute(object parameter)
    {
        _action.Executor.Execute(Target, parameter);
    }

    /// <summary>
    /// Gets the message handler attributes.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type.</typeparam>
    /// <returns>Collection of <typeparamref name="TAttribute"/>.</returns>
    public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute
    {
        return _action.Attributes.Where(x => x is TAttribute).Cast<TAttribute>();
    }
}
