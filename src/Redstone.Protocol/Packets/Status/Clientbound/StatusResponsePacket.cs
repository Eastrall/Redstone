using Redstone.Common.Server;
using System.Text.Json;

namespace Redstone.Protocol.Packets.Status.Clientbound
{
    public class StatusResponsePacket : MinecraftPacket
    {
        public StatusResponsePacket(MinecraftServerStatus serverStatus)
            : base(StatusClientboundPacketType.Response)
        {
            string serverStatusJson = JsonSerializer.Serialize(serverStatus, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            WriteString(serverStatusJson);
        }
    }
}
