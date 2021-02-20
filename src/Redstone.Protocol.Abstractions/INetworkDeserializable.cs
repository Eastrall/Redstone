namespace Redstone.Protocol.Abstractions
{
    /// <summary>
    /// Provides a mechanism that allows packet deserialization.
    /// </summary>
    public interface INetworkDeserializable
    {
        /// <summary>
        /// Deserializes the current object using the given minecraft packet stream.
        /// </summary>
        /// <param name="packet">Minecraft packet stream.</param>
        void Deserialize(IMinecraftPacket packet);
    }
}
