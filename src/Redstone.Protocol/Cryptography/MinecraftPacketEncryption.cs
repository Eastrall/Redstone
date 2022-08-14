using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Cryptography.ASN;
using System.Security.Cryptography;

namespace Redstone.Protocol.Cryptography;

/// <summary>
/// Minecraft packet encryption implementation.
/// </summary>
internal class MinecraftPacketEncryption : IMinecraftPacketEncryption
{
    public MinecraftEncryptionDetails GenerateEncryption(RSAParameters parameters)
    {
        byte[] key = PublicKeyToX509(parameters);
        var verifyToken = new byte[4];
        var csp = new RNGCryptoServiceProvider();
        csp.GetBytes(verifyToken);

        return new MinecraftEncryptionDetails(key, verifyToken);
    }

    public RSAParameters GenerateEncryptionKeys()
    {
        RSAParameters parameters;
        using var rsa = new RSACryptoServiceProvider(1024);

        try
        {
            parameters = rsa.ExportParameters(true);
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }

        return parameters;
    }

    private byte[] PublicKeyToX509(RSAParameters publicKey)
    {
        AsnType oid = AsnType.CreateOid("1.2.840.113549.1.1.1");
        AsnType algorithmID = AsnType.CreateSequence(new AsnType[] { oid, AsnType.CreateNull() });

        AsnType n = AsnType.CreateIntegerPos(publicKey.Modulus);
        AsnType e = AsnType.CreateIntegerPos(publicKey.Exponent);
        AsnType key = AsnType.CreateBitString(AsnType.CreateSequence(new AsnType[] { n, e }));
        AsnType publicKeyInfo = AsnType.CreateSequence(new AsnType[] { algorithmID, key });

        return new AsnMessage(publicKeyInfo.GetBytes(), "X.509").GetBytes();
    }
}
