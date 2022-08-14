namespace Redstone.Protocol.Packets.Game.Client;

public class UpdateViewPositionPacket : MinecraftPacket
{
    /// <summary>
    /// Creates a new <see cref="UpdateViewPositionPacket"/> instance.
    /// </summary>
    /// <param name="chunkPositionX">Chunk X coordinate of the player's position.</param>
    /// <param name="chunkPositionZ">Chunk Z coordinate of the player's position.</param>
    public UpdateViewPositionPacket(int chunkPositionX, int chunkPositionZ)
        : base(ClientPlayPacketType.UpdateViewPosition)
    {
        WriteVarInt32(chunkPositionX);
        WriteVarInt32(chunkPositionZ);
    }
}
