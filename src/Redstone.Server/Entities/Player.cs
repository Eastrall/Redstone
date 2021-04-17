﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Common.Configuration;
using Redstone.Common.Utilities;
using Redstone.Protocol.Packets.Game.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Redstone.Server.Entities
{
    [DebuggerDisplay("{Name}")]
    internal class Player : WorldEntity, IPlayer
    {
        private readonly IMinecraftUser _user;
        private readonly Queue<long> _keepAliveIdQueue;
        private readonly IOptions<GameOptions> _gameOptions;

        public override Guid Id { get; }

        public string Name { get; private set; }

        public int Ping { get; private set; }

        public ServerGameModeType GameMode { get; internal set; }

        public Player(IMinecraftUser user, Guid id, string name, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _user = user;
            Id = id;
            Name = name;
            _keepAliveIdQueue = new Queue<long>();
            _gameOptions = serviceProvider.GetRequiredService<IOptions<GameOptions>>();
        }

        public override void SendPacket(IMinecraftPacket packet) => _user.Send(packet);

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
            var keepAliveId = TimeUtilities.GetElapsedMilliseconds();
            _keepAliveIdQueue.Enqueue(keepAliveId);

            using var keepAlivePacket = new KeepAlivePacket(keepAliveId);
            _user.Send(keepAlivePacket);
        }

        public void CheckKeepAlive(long keepAliveId)
        {
            if (_keepAliveIdQueue.TryDequeue(out long nextKeepAliveId))
            {
                if (nextKeepAliveId != keepAliveId)
                {
                    _user.Disconnect("Keep-alive id doesn't match.");
                }

                Ping = (int)(TimeUtilities.GetElapsedMilliseconds() - nextKeepAliveId);

                using var playerInfoLatencyPacket = new PlayerInfoPacket(PlayerInfoActionType.UpdateLatency, this);
                World.SendToAll(playerInfoLatencyPacket);
            }
        }

        public override void AddVisibleEntity(IEntity entity)
        {
            base.AddVisibleEntity(entity);

            using IMinecraftPacket packet = entity switch
            {
                IPlayer playerEntity => new SpawnPlayerPacket(playerEntity),
                // TODO: add other entity types
                _ => throw new NotImplementedException()
            };
            SendPacket(packet);
        }

        public override void RemoveVisibleEntity(IEntity entity)
        {
            base.RemoveVisibleEntity(entity);
            // TODO: send despawn packet
        }
    }
}
