using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Protocol;
using Redstone.Protocol.Packets.Game.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redstone.Server.Entities
{
    [DebuggerDisplay("{Name}")]
    internal class Player : WorldEntity, IPlayer
    {
        private readonly IMinecraftUser _user;
        private readonly Queue<long> _keepAliveIdQueue;

        public override Guid Id => _user.Id;

        public string Name { get; private set; }

        public Player(IMinecraftUser user)
        {
            _user = user;
            _keepAliveIdQueue = new Queue<long>();
        }

        public void SendPacket(IMinecraftPacket packet) => _user.Send(packet);

        public void SetName(string newName, bool notifyOtherPlayers = false)
        {
            Name = newName;

            if (notifyOtherPlayers)
            {
                // TODO: send to every players
            }
        }

        public void KeepAlive()
        {
            var keepAliveId = DateTime.UtcNow.Millisecond;
            _keepAliveIdQueue.Enqueue(keepAliveId);

            using var keepAlivePacket = new KeepAlivePacket(keepAliveId);
            _user.Send(keepAlivePacket);
        }

        public void CheckKeepAlive(long keepAliveId)
        {
            var nextKeepAliveId = _keepAliveIdQueue.Dequeue();

            if (nextKeepAliveId != keepAliveId)
            {
                _user.Disconnect("Keep-alive id doesn't match.");
            }
        }

        public override void AddVisibleEntity(IEntity entity)
        {
            base.AddVisibleEntity(entity);
            // TODO: send spawn packet
        }

        public override void RemoveVisibleEntity(IEntity entity)
        {
            base.RemoveVisibleEntity(entity);
            // TODO: send despawn packet
        }
    }
}
