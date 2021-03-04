using Redstone.Abstractions.Entities;
using Redstone.Abstractions.World;
using Redstone.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Server.Entities
{
    internal class WorldEntity : IEntity
    {
        private static readonly object _entityIdGeneratorLock = new object();
        private static int _entityIdGenerator = 1;

        private readonly IList<IEntity> _visibleEntities;

        public virtual Guid Id { get; } = Guid.NewGuid();

        public int EntityId { get; }

        public Position Position { get; } = new Position();

        public float Angle { get; set; }

        public float HeadAngle { get; set; }

        public IWorldMap Map { get; internal set; }

        public IEnumerable<IEntity> VisibleEntities => _visibleEntities;

        public WorldEntity()
        {
            lock (_entityIdGeneratorLock)
            {
                EntityId = _entityIdGenerator++;
            }

            _visibleEntities = new List<IEntity>();
        }

        public void LookAround()
        {
            IEnumerable<IEntity> currentVisibleEntities = Map.GetVisibleEntities(this);
            IEnumerable<IEntity> appearingEntities = currentVisibleEntities.Except(_visibleEntities);
            IEnumerable<IEntity> disapearingEntities = _visibleEntities.Except(currentVisibleEntities);

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
            if (!_visibleEntities.Contains(entity))
            {
                _visibleEntities.Add(entity);
            }
        }

        public virtual void RemoveVisibleEntity(IEntity entity)
        {
            if (_visibleEntities.Contains(entity))
            {
                _visibleEntities.Remove(entity);
            }
        }

        public bool Equals(IEntity other) => other is not null && EntityId == other.EntityId;
    }
}
