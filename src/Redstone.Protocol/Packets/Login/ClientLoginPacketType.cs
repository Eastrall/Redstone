namespace Redstone.Protocol.Packets.Login;

public enum ClientLoginPacketType
{
    Disconnect = 0x00,
    EncryptionRequest = 0x01,
    LoginSuccess = 0x02,
    SetCompression = 0x03,
    LoginPluginRequest = 0x04
}
