using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Protocol.Packets.Game.Client;
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

        private Position _lastPosition;

        public virtual Guid Id { get; } = Guid.NewGuid();

        public int EntityId { get; }

        public bool IsSpawned { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public bool IsOnGround { get; private set; }

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

        public virtual void Move(Position destinationPosition, bool isOnGround)
        {
            if (!IsSpawned)
            {
                return;
            }

            var deltaX = ((destinationPosition.X * 32) - (Position.X * 32)) * 128;
            var deltaY = ((destinationPosition.Y * 32) - (Position.Y * 32)) * 128;
            var deltaZ = ((destinationPosition.Z * 32) - (Position.Z * 32)) * 128;
            var delta = new Position(deltaX, deltaY, deltaZ);

            Position.Copy(destinationPosition);

            //_lastPosition.Copy(Position);
            IsOnGround = true; // TODO: check block at current position. If not air then true, false otherwise.

            using var movePacket = new EntityPositionPacket(this, delta);
            SendPacketToVisibleEntities(movePacket);
        }

        public virtual void Rotate(float yawAngle, float pitchAngle)
        {
            if (!IsSpawned)
            {
                return;
            }

            Angle = yawAngle;
            HeadAngle = pitchAngle;

            using var entityRotationPacket = new EntityRotationPacket(EntityId, Angle, HeadAngle, IsOnGround);
            using var entityHeadLookPacket = new EntityHeadLookPacket(EntityId, Angle);

            SendPacketToVisibleEntities(entityRotationPacket);
            SendPacketToVisibleEntities(entityHeadLookPacket);
        }

        public void MoveAndRotate(Position destinationPosition, float yawAngle, float pitchAngle, bool isOnGround)
        {
            if (!IsSpawned)
            {
                return;
            }

            var deltaX = ((destinationPosition.X * 32) - (Position.X * 32)) * 128;
            var deltaY = ((destinationPosition.Y * 32) - (Position.Y * 32)) * 128;
            var deltaZ = ((destinationPosition.Z * 32) - (Position.Z * 32)) * 128;
            var delta = new Position(deltaX, deltaY, deltaZ);

            Position.Copy(destinationPosition);
            Angle = yawAngle;
            HeadAngle = pitchAngle;
            IsOnGround = true; // TODO: check block at current position. If not air then true, false otherwise.

            using var entityMovePacket = new EntityPositionAndRotationPacket(this, delta);
            SendPacketToVisibleEntities(entityMovePacket);
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
