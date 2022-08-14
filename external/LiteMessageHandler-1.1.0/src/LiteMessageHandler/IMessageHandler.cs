namespace LiteMessageHandler;

/// <summary>
/// Provides a mechanism to define a class as a message handler that can be invoked through the <see cref="MessageHandlerDispatcher"/>.
/// </summary>
/// <typeparam name="TMessage">Message type to handle.</typeparam>
public interface IMessageHandler<TMessage>
{
    /// <summary>
    /// Executes the message.
    /// </summary>
    /// <param name="message">Message.</param>
    void Execute(TMessage message);
}
