using Redstone.Common.Server;
using System.Text.Json;

namespace Redstone.Protocol.Packets.Status.Client;

/// <summary>
/// Defines the Minecraft status response packet structure.
/// </summary>
public class StatusResponsePacket : MinecraftPacket
{
    /// <summary>
    /// Creates a new <see cref="StatusResponsePacket"/> instance.
    /// </summary>
    /// <param name="serverStatus">Server status information.</param>
    public StatusResponsePacket(MinecraftServerStatus serverStatus)
        : base(ClientStatusPacketType.Response)
    {
        var serverStatusJson = JsonSerializer.Serialize(serverStatus, options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        WriteString(serverStatusJson);
    }
}
