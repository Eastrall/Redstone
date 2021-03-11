using System.Security.Cryptography;

namespace Redstone.Abstractions.Protocol
{
    /// <summary>
    /// Provides a mechanism to manage minecraft packet encryption.
    /// </summary>
    public interface IMinecraftPacketEncryption
    {
        /// <summary>
        /// Generates the encryption keys.
        /// </summary>
        /// <remarks>
        /// More information here: https://wiki.vg/Protocol_Encryption
        /// </remarks>
        RSAParameters GenerateEncryptionKeys();

        //MinecraftEncryptionDetails GenerateEncryption(RSAParameters parameters);
    }
}
