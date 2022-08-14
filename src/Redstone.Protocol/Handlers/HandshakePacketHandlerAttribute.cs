using Redstone.Abstractions.Protocol;
using System;

namespace Redstone.Protocol.Handlers;

/// <summary>
/// Provides an attribute that indicates that the method annotated with this attribute should
/// be invoked only during  <see cref="MinecraftUserStatus.Handshaking"/> state.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HandshakePacketHandlerAttribute : PacketHandlerAttribute
{
    /// <summary>
    /// Creates a new <see cref="HandshakePacketHandlerAttribute"/> instance.
    /// </summary>
    /// <param name="action">Handler action type.</param>
    public HandshakePacketHandlerAttribute(object action)
        : base(MinecraftUserStatus.Handshaking, action)
    {
    }
}
