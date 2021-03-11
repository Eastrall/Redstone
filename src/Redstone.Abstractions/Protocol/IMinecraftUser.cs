using System;

namespace Redstone.Abstractions.Protocol
{
    /// <summary>
    /// Provides a mechanism to handle a miencraft user connection.
    /// </summary>
    public interface IMinecraftUser
    {
        /// <summary>
        /// Gets the Minecraft user id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the Miencraft user current status.
        /// </summary>
        MinecraftUserStatus Status { get; }

        /// <summary>
        /// Sends the given packet to the current user.
        /// </summary>
        /// <param name="packet">Packet to be sent.</param>
        void Send(IMinecraftPacket packet);

        /// <summary>
        /// Disconnects the current user.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Disconnects the current user with a given reason.
        /// </summary>
        /// <param name="reason">Disconnect reason.</param>
        void Disconnect(string reason);
    }
}
