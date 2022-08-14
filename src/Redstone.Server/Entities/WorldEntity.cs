using Microsoft.Extensions.DependencyInjection;
using Redstone.Abstractions.Entities;
using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.IO;
using Redstone.Protocol;
using Redstone.Protocol.Packets.Game.Client;
using Redstone.Server.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public Position Position { get; init; } = new Position();

        public float Angle { get; set; }

        public float HeadAngle { get; set; }

        public IWorldMap Map { get; internal set; }

        public IChunk Chunk => GetActualChunk();

        public IEnumerable<IEntity> VisibleEntities => _visibleEntities.Values;

        public IWorld World { get; protected set; }

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

        [ExcludeFromCodeCoverage]
        public virtual void SendPacket(IMinecraftPacket packet)
        {
            // Nothing to do.
        }

        public virtual void SendPacketToVisibleEntities(IMinecraftPacket packet, bool includeEntity = false)
        {
            if (packet is null)
            {
                throw new ArgumentNullException(nameof(packet), "The packet is null.");
            }

            if (includeEntity)
            {
                SendPacket(packet);
            }

            foreach (IEntity entity in VisibleEntities)
            {
                entity.SendPacket(packet);
            }
        }

        public void LookAround()
        {
            if (!IsSpawned || !IsVisible)
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
            IsOnGround = GetBlock().GetRelative(BlockFaceType.Bottom).IsSolid;

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
            IsOnGround = !GetBlock().GetRelative(BlockFaceType.Bottom).IsAir;

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

        private IChunk GetActualChunk()
        {
            int regionX = (int)Position.X / Region.Size;
            int regionZ = (int)Position.Z / Region.Size;
            IRegion region = Map.GetRegion(regionX, regionZ);

            if (region is null)
            {
                throw new InvalidOperationException($"Failed to get region at coordinates: {regionX}/{regionZ}");
            }

            int regionPlayerPosX = (int)Position.X % Region.Size;
            int regionPlayerPosZ = (int)Position.Z % Region.Size;
            int chunkX = regionPlayerPosX / Redstone.Server.World.Chunk.Size;
            int chunkZ = regionPlayerPosZ / Redstone.Server.World.Chunk.Size;

            IChunk chunk = region.GetChunk(chunkX, chunkZ);

            if (chunk is null)
            {
                throw new InvalidOperationException($"Failed to get chunk {chunkX}/{chunkZ} from region {regionX}/{regionZ}");
            }

            return chunk;
        }

        /// <summary>
        /// Gets the block where the current world object is located.
        /// </summary>
        /// <returns></returns>
        private IBlock GetBlock() => Chunk.GetBlock((int)Position.X, (int)Position.Y, (int)Position.Z);
    }
}
