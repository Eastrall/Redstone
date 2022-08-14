using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LiteMessageHandler.Sample.Hosting;

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

