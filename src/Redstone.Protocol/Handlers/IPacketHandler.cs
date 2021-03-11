using Redstone.Abstractions.Protocol;

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
        /// <param name="status">User status.</param>
        /// <param name="handler">Packet handler to invoke during the given status.</param>
        /// <param name="parameters">Additionnal parameters to pass to the packet handler action.</param>
        object Invoke(MinecraftUserStatus status, object handler, params object[] parameters);
    }
}
