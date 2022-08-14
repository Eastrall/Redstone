using Redstone.Abstractions.Protocol;
using System;

namespace Redstone.Protocol.Handlers;

/// <summary>
/// Provides an attribute that indicates that the method annotated with this attribute should
/// be invoked only during <see cref="MinecraftUserStatus.Play"/> state.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PlayPacketHandlerAttribute : PacketHandlerAttribute
{
    /// <summary>
    /// Creates a new <see cref="PlayPacketHandlerAttribute"/> instance.
    /// </summary>
    /// <param name="action">Handler action type.</param>
    public PlayPacketHandlerAttribute(object action)
        : base(MinecraftUserStatus.Status, action)
    {

    }
}
