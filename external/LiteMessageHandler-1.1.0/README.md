# LiteMessageHandler

[![.NET](https://github.com/Eastrall/LiteMessageHandler/actions/workflows/build.yml/badge.svg)](https://github.com/Eastrall/LiteMessageHandler/actions/workflows/build.yml)

`LiteMessageHandler` is a simple messaging system.

## How it works

```csharp
using LiteMessageHandler;
using System;

internal class Program
{
    static void Main()
    {
        var messageDispatcher = new MessageHandlerDispatcher();
        
        // Executes the handler that have the "PersonRequest" generic parameter.
        // In this case: PersonRequestHandler.
        messageDispatcher.Dispatch(new PersonRequest
        {
            FirstName = "John",
            LastName = "Doe"
        });
    }
}

public class PersonRequestHandler : MessageHandler<PersonRequest>
{
    public override void Execute(PersonRequest message)
    {
        Console.WriteLine($"PersonRequest: {message.FirstName} {message.LastName}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
```

This example will display the following output:

```
PersonRequest: John Doe
```

## Advanced usages

### Work with the target handler

`LiteMessageHandler` allows you to retrieve the instance of a handler and manipulate it before you actually dispatch its action. To do so, just call the `GetHandler(Type)` or ` GetHandler<T>()` method to get a `MessageHandler` object.

```csharp
using LiteMessageHandler;
using System;

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

```

Displays the following output:
```
PersonRequest: John Doe | Custom Value = This is my custom value!
```

### Dependency injection

You can use the `MessageHandlerDispatcher` within a [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) and take advantage of the built-in dependency injection system. You will be able to inject any kind of service in your handlers.

```csharp
using LiteMessageHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

internal class Program
{
    static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IConsoleService, ConsoleService>();
                services.AddSingleton<IMessageHandlerDispatcher, MessageHandlerDispatcher>();
            })
            .Build();

        var dispatcher = host.Services.GetRequiredService<IMessageHandlerDispatcher>();

        dispatcher.Dispatch(new PersonRequest
        {
            FirstName = "John",
            LastName = "Doe"
        });
    }
}

public interface IConsoleService
{
    void WriteLine(string message);
}

public class ConsoleService : IConsoleService
{
    public void WriteLine(string message) => Console.WriteLine(message);
}

public class PersonRequestHandler : IMessageHandler<PersonRequest>
{
    private readonly IConsoleService _consoleService;

    public PersonRequestHandler(IConsoleService consoleService)
    {
        _consoleService = consoleService;
    }

    public void Execute(PersonRequest message)
    {
        _consoleService.WriteLine($"PersonRequest: {message.FirstName} {message.LastName}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
```

### Custom attribute metadata support

In some cases, you might want to annotate your handlers to be able to apply special treatements when you invoke them.
LiteMessageHandler now provides a way to retrieve the handler's attributes.

```csharp
using LiteMessageHandler;
using System;
using System.Linq;

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
```

This snippet above will display the following output:
```
Handler attribute value = 'This is a custom person attribute!'
PersonRequest: John Doe
```