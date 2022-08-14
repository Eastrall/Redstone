using System;

namespace LiteMessageHandler.Sample.GetHandler;

internal class Program
{
    static void Main()
    {
        MessageHandlerDispatcher? messageDispatcher = new();
        MessageHandler? handler = messageDispatcher.GetHandler<PersonRequest>();

        if (handler != null)
        {
            // Here we can convert the actual handler Target into a "CustomHandler" instance
            // since the actual target is a "PersonRequestHandler" and extends from "CustomHandler".
            CustomHandler? messageHandler = handler.Target as CustomHandler;

            if (messageHandler != null)
            {
                messageHandler.CustomValue = "This is my custom value!";
            }

            // After working with the hander's target instance, we can simply execute the handler.
            handler.Execute(new PersonRequest
            {
                FirstName = "John",
                LastName = "Doe"
            });
        }
    }
}

public class PersonRequestHandler : CustomHandler, IMessageHandler<PersonRequest>
{
    public void Execute(PersonRequest message)
    {
        Console.WriteLine($"PersonRequest: {message.FirstName} {message.LastName} | Custom Value = {CustomValue}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class CustomHandler
{
    public string? CustomValue { get; set; }
}
