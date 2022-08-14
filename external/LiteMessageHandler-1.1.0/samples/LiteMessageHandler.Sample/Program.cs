using System;

namespace LiteMessageHandler.Sample;

internal class Program
{
    static void Main()
    {
        var messageDispatcher = new MessageHandlerDispatcher();

        messageDispatcher.Dispatch(new PersonRequest
        {
            FirstName = "John",
            LastName = "Doe"
        });
    }
}

public class PersonRequestHandler : IMessageHandler<PersonRequest>
{
    public void Execute(PersonRequest message)
    {
        Console.WriteLine($"PersonRequest: {message.FirstName} {message.LastName}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
