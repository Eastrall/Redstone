namespace Redstone.Protocol.Abstractions
{
    /// <summary>
    /// Provides a mechanism that allows packet serialization.
    /// </summary>
    public interface INetworkSerializable
    {
        /// <summary>
        /// Serializes the current object into the given minecraft packet stream.
        /// </summary>
        /// <param name="packet">Minecraft packet stream.</param>
        void Serialize(IMinecraftPacket packet);
    }
}
