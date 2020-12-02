using System.Security.Cryptography;

namespace Redstone.Server
{
    /// <summary>
    /// Provides an interface to manage the Redstone server instance.
    /// </summary>
    public interface IRedstoneServer
    {
        RSAParameters ServerEncryptionKey { get; }
    }
}
