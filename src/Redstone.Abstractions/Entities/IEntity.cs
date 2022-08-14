using Redstone.Abstractions.Protocol;
using Redstone.Abstractions.World;
using Redstone.Common;
using Redstone.Common.IO;
using System;
using System.Collections.Generic;

namespace Redstone.Abstractions.Entities;

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
    /// Gets or sets a boolean value that indicates if the current entity is spawned.
    /// </summary>
    bool IsSpawned { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the current entity is visible.
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Gets a boolean value that indicates if the current entity is on ground.
    /// </summary>
    bool IsOnGround { get; }

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
    /// Gets the current world.
    /// </summary>
    IWorld World { get; }

    /// <summary>
    /// Gets the current entity map.
    /// </summary>
    IWorldMap Map { get; }

    /// <summary>
    /// Gets the current entity actual chunk.
    /// </summary>
    IChunk Chunk { get; }

    /// <summary>
    /// Gets the visible entities from the current entity position.
    /// </summary>
    IEnumerable<IEntity> VisibleEntities { get; }

    /// <summary>
    /// The current entity looks around and updates the current entity visible entities list.
    /// </summary>
    void LookAround();

    /// <summary>
    /// Moves the current entity to the given destination position and notifies other entities around.
    /// </summary>
    /// <param name="destinationPosition">Destination position.</param>
    /// <param name="isOnGround">Boolean value that indicates if the entity is on ground.</param>
    void Move(Position destinationPosition, bool isOnGround);

    /// <summary>
    /// Rotates the current entity using the given angles.
    /// </summary>
    /// <param name="yawAngle">Yaw angle.</param>
    /// <param name="pitchAngle">Pitch angle.</param>
    void Rotate(float yawAngle, float pitchAngle);

    /// <summary>
    /// Moves the current entity to the given destination position and rotates it using the given angles.
    /// Also notifies the other entities around.
    /// </summary>
    /// <param name="destinationPosition">Destination position.</param>
    /// <param name="yawAngle">Yaw angle.</param>
    /// <param name="pitchAngle">Pitch angle.</param>
    /// <param name="isOnGround">Boolean value that indicates if the entity is on ground.</param>
    void MoveAndRotate(Position destinationPosition, float yawAngle, float pitchAngle, bool isOnGround);

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
    /// <param name="includeEntity">Boolean value that indicates if the packet should also be sent to the current entity.</param>
    void SendPacketToVisibleEntities(IMinecraftPacket packet, bool includeEntity = false);
}
