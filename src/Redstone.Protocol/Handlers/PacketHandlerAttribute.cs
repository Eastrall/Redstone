using Redstone.Abstractions.Protocol;
using System;

namespace Redstone.Protocol.Handlers
{
    public class PacketHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets the minecraft user status that indicates when the handler can be invoked.
        /// </summary>
        public MinecraftUserStatus Status { get; }
        
        /// <summary>
        /// Gets the handler action.
        /// </summary>
        public object Action { get; }

        /// <summary>
        /// Creates a new <see cref="PacketHandlerAttribute"/> instance.
        /// </summary>
        /// <param name="status">Minecraft user status when the handler can be invoked.</param>
        /// <param name="action">Handler action.</param>
        protected PacketHandlerAttribute(MinecraftUserStatus status, object action)
        {
            Status = status;
            Action = action;
        }
    }
}
