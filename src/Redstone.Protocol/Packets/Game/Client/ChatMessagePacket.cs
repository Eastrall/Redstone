using Redstone.Abstractions.Entities;
using Redstone.Common.Chat;

namespace Redstone.Protocol.Packets.Game.Client;

public class ChatMessagePacket : MinecraftPacket
{
    public ChatMessagePacket(IPlayer sender, ChatMessage chatMessage, ChatMessageType chatMessageType)
        : base(ClientPlayPacketType.ChatMessage)
    {
        WriteJson(chatMessage);
        WriteByte((byte)chatMessageType);
        WriteUUID(sender.Id);
    }
}
