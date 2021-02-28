using Redstone.Abstractions.Entities;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Protocol;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redstone.Server.Entities
{
    [DebuggerDisplay("{Name}")]
    internal class Player : IPlayer
    {
        private readonly MinecraftUser _user;
        private readonly Queue<long> _keepAliveIdQueue;

        public string Name { get; private set; }

        public Guid Id => _user.Id;

        public int EntityId { get; } = RandomHelper.GenerateEntityObjectId();

        public Position Position { get; } = new Position();

        public float Angle { get; set; }

        public float HeadAngle { get; set; }

        public IWorldMap Map { get; internal set; }

        public IEnumerable<IEntity> VisibleEntities { get; }

        public Player(MinecraftUser user)
        {
            _user = user;
            _keepAliveIdQueue = new Queue<long>();
        }

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
    }
}
