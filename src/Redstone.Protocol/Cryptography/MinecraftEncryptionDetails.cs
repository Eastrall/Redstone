namespace Redstone.Protocol.Cryptography;

public struct MinecraftEncryptionDetails
{
    public byte[] Key { get; }

    public byte[] VerifyToken { get; }

    public MinecraftEncryptionDetails(byte[] key, byte[] verifyToken)
    {
        Key = key;
        VerifyToken = verifyToken;
    }
}
