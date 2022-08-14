using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers;
using Redstone.Protocol.Packets.Game;

namespace Redstone.Server.Handlers.Play;

public class ChatHandler
{
    [PlayPacketHandler(ServerPlayPacketType.ChatMessage)]
    public void OnChatMessage(IMinecraftUser user, IMinecraftPacket packet)
    {
        string message = packet.ReadString();

        if (message.Length > 256)
        {
            user.Disconnect("Chat message exceeds 256 characters.");
        }

        if (message.StartsWith(RedstoneContants.CommandPrefix))
        {
            // TODO: interpret commands
        }
        else
        {
            user.Player.Speak(message);
        }
    }
}
