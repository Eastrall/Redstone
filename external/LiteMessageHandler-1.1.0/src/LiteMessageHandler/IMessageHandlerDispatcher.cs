using System;

namespace LiteMessageHandler;

/// <summary>
/// Provides a mechanism to manage the message handlers.
/// </summary>
public interface IMessageHandlerDispatcher : IDisposable
{
    MessageHandler? GetHandler(Type? type);

    MessageHandler? GetHandler<TMessage>() where TMessage : class;

    void Dispatch<TMessage>(TMessage message) where TMessage : class;
}
