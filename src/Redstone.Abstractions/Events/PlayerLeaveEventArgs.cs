using Redstone.Abstractions.Entities;
using System;

namespace Redstone.Abstractions.Events;

public class PlayerLeaveEventArgs : EventArgs
{
    public IPlayer Player { get; }

    public PlayerLeaveEventArgs(IPlayer player)
    {
        Player = player;
    }
}
