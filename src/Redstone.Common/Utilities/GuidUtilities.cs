using System;
using System.Security.Cryptography;
using System.Text;

namespace Redstone.Common.Utilities;

/// <summary>
/// Provides utility methods for the <see cref="Guid"/> type.
/// </summary>
public static class GuidUtilities
{
    /// <summary>
    /// Generates a <see cref="Guid"/> based on a byte array input.
    /// </summary>
    /// <param name="input">Byte array input.</param>
    /// <returns><see cref="Guid"/></returns>
    /// <remarks>
    /// Original code from https://stackoverflow.com/a/18023109/4717662
    /// </remarks>
    public static Guid GenerateGuidFromBytes(byte[] input)
    {
        MD5 md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(input);
        hash[6] &= 0x0f;
        hash[6] |= 0x30;
        hash[8] &= 0x3f;
        hash[8] |= 0x80;

        byte temp = hash[6];
        hash[6] = hash[7];
        hash[7] = temp;

        temp = hash[4];
        hash[4] = hash[5];
        hash[5] = temp;

        temp = hash[0];
        hash[0] = hash[3];
        hash[3] = temp;

        temp = hash[1];
        hash[1] = hash[2];
        hash[2] = temp;

        return new Guid(hash);
    }

    /// <summary>
    /// Generates a <see cref="Guid"/> based on the given input string.
    /// </summary>
    /// <param name="input">Input string.</param>
    /// <returns><see cref="Guid"/></returns>
    public static Guid GenerateGuidFromString(string input) => GenerateGuidFromBytes(Encoding.UTF8.GetBytes(input));
}
