using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Redstone.NBT.Tests.Utilities;

public static class FileAssert
{
    static string GetFileHash(string filename)
    {
        Assert.True(File.Exists(filename));

        using var hash = new SHA1Managed();
        
        var clearBytes = File.ReadAllBytes(filename);
        var hashedBytes = hash.ComputeHash(clearBytes);
        
        return ConvertBytesToHex(hashedBytes);
    }

    static string ConvertBytesToHex(byte[] bytes)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x"));
        }

        return sb.ToString();
    }

    public static void Equal(string filename1, string filename2)
    {
        string hash1 = GetFileHash(filename1);
        string hash2 = GetFileHash(filename2);

        Assert.Equal(hash1, hash2);
    }
}
