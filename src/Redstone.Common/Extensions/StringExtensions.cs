using System;
using System.Linq;

namespace Redstone.Common.Extensions
{
    /// <summary>
    /// Provides extensions to the string object.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the given string into a PascalCase notation string.
        /// </summary>
        /// <param name="source">Source string to convert.</param>
        /// <returns>String converted to Pascal Case notation.</returns>
        public static string ToPascalCase(this string source)
        {
            return source.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s[1..])
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
