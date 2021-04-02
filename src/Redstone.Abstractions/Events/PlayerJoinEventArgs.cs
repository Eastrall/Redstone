using Redstone.Abstractions.Entities;
using System;

namespace Redstone.Abstractions.Events
{
    public class PlayerJoinEventArgs : EventArgs
    {
        public IPlayer Player { get; }

        public PlayerJoinEventArgs(IPlayer player)
        {
            Player = player;
        }
    }
}
