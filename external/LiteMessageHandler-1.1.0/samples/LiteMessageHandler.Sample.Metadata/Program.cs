using System;
using System.Linq;

namespace LiteMessageHandler.Sample.Metadata;

internal class Program
{
    static void Main()
    {
        MessageHandlerDispatcher? messageDispatcher = new();
        MessageHandler? handler = messageDispatcher.GetHandler<PersonRequest>();

        if (handler != null)
        {
            var attribute = handler.GetAttributes<CustomPersonAttribute>().First();

            Console.WriteLine($"Handler attribute value = '{attribute.Parameter}'");

            handler.Execute(new PersonRequest
            {
                FirstName = "John",
                LastName = "Doe"
            });
        }
    }
}

[CustomPerson("This is a custom person attribute!")]
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

[AttributeUsage(AttributeTargets.Class)]
public class CustomPersonAttribute : Attribute
{
    public string Parameter { get; }

    public CustomPersonAttribute(string parameter)
    {
        Parameter = parameter;
    }
}