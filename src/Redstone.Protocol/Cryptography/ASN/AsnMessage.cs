namespace Redstone.Protocol.Cryptography.ASN;

/// <summary>
/// ASN message format.
/// </summary>
/// <remarks>
/// Adapted from: https://github.com/jrnker/CSharp-easy-RSA-PEM/blob/master/CSharp-easy-RSA-PEM/CSharp-easy-RSA-PEM/AsnKeyBuilder.cs
/// all credits to the original author.
/// </remarks>
internal class AsnMessage
{
    private readonly byte[] _octets;
    private readonly string _format;

    internal int Length => null == _octets ? 0 : _octets.Length;

    internal AsnMessage(byte[] octets, string format)
    {
        _octets = octets;
        _format = format;
    }

    internal byte[] GetBytes() => _octets ?? (new byte[] { });

    internal string GetFormat() => _format;
}
