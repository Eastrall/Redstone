using Redstone.Abstractions.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Protocol.Packets.Game.Client;

public class DestroyEntitiesPacket : MinecraftPacket
{
    /// <summary>
    /// Creates a new <see cref="DestroyEntitiesPacket"/> for only one entity.
    /// </summary>
    /// <param name="entityToDestroy">Entity to destroy.</param>
    public DestroyEntitiesPacket(IEntity entityToDestroy)
        : this(new[] { entityToDestroy })
    {
    }

    /// <summary>
    /// Creates a new <see cref="DestroyEntitiesPacket"/> for the given entities list.
    /// </summary>
    /// <param name="entitiesToDestroy">Entities to destroy.</param>
    public DestroyEntitiesPacket(IEnumerable<IEntity> entitiesToDestroy)
        : base(ClientPlayPacketType.DestroyEntities)
    {
        WriteVarInt32(entitiesToDestroy.Count());

        foreach (IEntity entity in entitiesToDestroy)
        {
            WriteVarInt32(entity.EntityId);
        }
    }
}
