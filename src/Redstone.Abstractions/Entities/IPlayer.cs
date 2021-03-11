using Redstone.Abstractions.Protocol;

namespace Redstone.Abstractions.Entities
{
    /// <summary>
    /// Provides an interface that describes the player behavior.
    /// </summary>
    public interface IPlayer : IEntity
    {
        /// <summary>
        /// Gets the player name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sends a packet to the current player.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        void SendPacket(IMinecraftPacket packet);

        /// <summary>
        /// Sets the current player name.
        /// </summary>
        /// <param name="newName">New name.</param>
        /// <param name="notifyOtherPlayers">
        /// Boolean value that indicates if the new name should be broadcasted to every player on the server.
        /// </param>
        void SetName(string newName, bool notifyOtherPlayers = false);

        /// <summary>
        /// Keeps the current player alive by sending a "keep-alive" packet.
        /// </summary>
        void KeepAlive();

        /// <summary>
        /// Checks if the given keep-alive id is the same has the one sent by the current player.
        /// </summary>
        /// <param name="keepAliveId">Keep alive id.</param>
        void CheckKeepAlive(long keepAliveId);
    }
}
