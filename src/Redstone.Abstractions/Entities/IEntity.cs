using System;

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
        /// Gets the entity position.
        /// </summary>
        Position Position { get; }
    }
}
