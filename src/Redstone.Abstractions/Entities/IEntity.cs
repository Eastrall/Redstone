using Redstone.Abstractions.World;
using Redstone.Common;
using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.Entities
{
    /// <summary>
    /// Provides an interface that describes the base entity behavior.
    /// </summary>
    public interface IEntity
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
    }
}
