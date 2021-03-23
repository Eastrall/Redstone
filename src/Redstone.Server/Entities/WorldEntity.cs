using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server.Entities
{
    internal class WorldEntity : IEntity
    {
        private static readonly object _entityIdGeneratorLock = new();
        private static int _entityIdGenerator = 1;

        private readonly ConcurrentDictionary<Guid, IEntity> _visibleEntities;
        private readonly IServiceProvider _serviceProvider;

        public virtual Guid Id { get; } = Guid.NewGuid();

        public int EntityId { get; }

        public bool IsSpawned { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public Position Position { get; } = new Position();

        public float Angle { get; set; }

        public float HeadAngle { get; set; }

        public IWorldMap Map { get; internal set; }

        public IEnumerable<IEntity> VisibleEntities => _visibleEntities.Values;

        protected IWorld World { get; }

        public WorldEntity(IServiceProvider serviceProvider)
        {
            lock (_entityIdGeneratorLock)
            {
                EntityId = _entityIdGenerator++;
            }

            _visibleEntities = new ConcurrentDictionary<Guid, IEntity>();
            _serviceProvider = serviceProvider;

            World = _serviceProvider.GetRequiredService<IWorld>();
        }

        public virtual void SendPacket(IMinecraftPacket packet)
        {
            // Nothing to do.
        }

        public virtual void SendPacketToVisibleEntities(IMinecraftPacket packet)
        {
            if (packet is null)
            {
                throw new ArgumentNullException(nameof(packet), "The packet is null.");
            }

            foreach (IEntity entity in VisibleEntities)
            {
                entity.SendPacket(packet);
            }
        }

        public void LookAround()
        {
            if (!IsSpawned)
            {
                return;
            }

            IEnumerable<IEntity> currentVisibleEntities = Map.GetVisibleEntities(this);
            IEnumerable<IEntity> appearingEntities = currentVisibleEntities.Except(_visibleEntities.Values);
            IEnumerable<IEntity> disapearingEntities = _visibleEntities.Values.Except(currentVisibleEntities);

            if (appearingEntities.Any() || disapearingEntities.Any())
            {
                foreach (IEntity appearingEntity in appearingEntities)
                {
                    AddVisibleEntity(appearingEntity);
                }

                foreach (IEntity disapearingEntity in disapearingEntities)
                {
                    RemoveVisibleEntity(disapearingEntity);
                }
            }
        }

        public virtual void AddVisibleEntity(IEntity entity)
        {
            if (!_visibleEntities.ContainsKey(entity.Id))
            {
                _visibleEntities.TryAdd(entity.Id, entity);
            }
        }

        public virtual void RemoveVisibleEntity(IEntity entity)
        {
            if (_visibleEntities.ContainsKey(entity.Id))
            {
                _visibleEntities.TryRemove(entity.Id, out _);
            }
        }

        public bool Equals(IEntity other) => other is not null && EntityId == other.EntityId;
    }
}
