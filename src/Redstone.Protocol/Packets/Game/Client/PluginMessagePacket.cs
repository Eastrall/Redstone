namespace Redstone.Protocol.Packets.Game.Client
{
    public class PluginMessagePacket : MinecraftPacket
    {
        public PluginMessagePacket(string channel)
            : base(ClientPlayPacketType.PluginMessage)
        {
            WriteString(channel);
        }
    }
}
