using System;

namespace Redstone.Protocol.Handlers
{
    /// <summary>
    /// Provides a mechanism to invoke a packet handler.
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        /// Invokes the correct packet handler attached to the given packet type during the given user status.
        /// </summary>
        /// <param name="invoker">Packet handler invoker.</param>
        /// <param name="status">User status.</param>
        /// <param name="packetType">Packet handler to invoke during the given status.</param>
        void Invoke(object invoker, MinecraftUserStatus status, object packetType);
    }
}
