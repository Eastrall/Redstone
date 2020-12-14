using System;

namespace Redstone.Protocol.Handlers
{
    /// <summary>
    /// Provides an attribute that indicates that the method annotated with this attribute should
    /// be invoked only during <see cref="MinecraftUserStatus.Status"/> state.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StatusPacketHandlerAttribute : PacketHandlerAttribute
    {
        /// <summary>
        /// Creates a new <see cref="StatusPacketHandlerAttribute"/> instance.
        /// </summary>
        /// <param name="action">Handler action type.</param>
        public StatusPacketHandlerAttribute(object action)
            : base(MinecraftUserStatus.Status, action)
        {

        }
    }
}
