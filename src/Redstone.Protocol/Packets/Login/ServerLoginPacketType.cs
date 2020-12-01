namespace Redstone.Protocol.Packets.Login
{
    public enum ServerLoginPacketType
    {
        LoginStart = 0x00,
        EncryptionResponse = 0x01,
        LoginPluginResponse = 0x02
    }
}
