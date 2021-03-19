using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.Entities
{
    /// <summary>
    /// Provides an interface that describes the base entity behavior.
    /// </summary>
    public interface IEntity : IEquatable<IEntity>
    {
        /// <summary>
        /// Gets the entity unique id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the entity object id.
        /// </summary>
        int EntityId { get; }

        /// <summary>
        /// Gets the entity position.
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Gets or sets the entity angle.
        /// </summary>
        /// <remarks>
        /// This value represents the YAW angle.
        /// </remarks>
        float Angle { get; set; }

        /// <summary>
        /// Gets or sets the entity head angle.
        /// </summary>
        /// <remarks>
        /// This value represents the PITCH angle.
        /// </remarks>
        float HeadAngle { get; set; }

        /// <summary>
        /// Gets the current entity map.
        /// </summary>
        IWorldMap Map { get; }

        /// <summary>
        /// Gets the visible entities from the current entity position.
        /// </summary>
        IEnumerable<IEntity> VisibleEntities { get; }

        /// <summary>
        /// The current entity looks around and updates the current entity visible entities list.
        /// </summary>
        void LookAround();

        /// <summary>
        /// Adds the given entity to the visible entities collection.
        /// </summary>
        /// <param name="entity">Entity that is visible by the current entity.</param>
        void AddVisibleEntity(IEntity entity);

        /// <summary>
        /// Removes the given entity from the visible entities collection.
        /// </summary>
        /// <param name="entity">Entity that is not visible anymore by the current entity.</param>
        void RemoveVisibleEntity(IEntity entity);

        /// <summary>
        /// Sends a packet to the current entity.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        void SendPacket(IMinecraftPacket packet);

        /// <summary>
        /// Sends a packet to all visible entities of the current entity.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        void SendPacketToVisibleEntities(IMinecraftPacket packet);
    }
}
