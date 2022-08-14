using Redstone.Abstractions.Entities;

namespace Redstone.Abstractions.Handlers;

public abstract class MinecraftHandler<TMessage> where TMessage : class
{
    protected IPlayer Player { get; }

    protected MinecraftHandler()
    {
    }

    public abstract void Execute(TMessage message);
}
