using System;

namespace Redstone.Common.Extensions
{
    /// <summary>
    /// Provides extensions for the primitive types such as int, short, long, etc...
    /// </summary>
    public static class PrimitiveTypeExtensions
    {
        /// <summary>
        /// Returns the number of bits needed for the current short value.
        /// </summary>
        /// <param name="value">Current short.</param>
        /// <returns>Number of bits needed for the current short value.</returns>
        public static int NeededBits(this short value)
        {
            return Convert.ToString(value, 2).Length;
        }

        /// <summary>
        /// Returns the number of bits needed for the current integer.
        /// </summary>
        /// <param name="value">Current integer.</param>
        /// <returns>Number of bits needed for the current integer.</returns>
        public static int NeededBits(this int value)
        {
            return Convert.ToString(value, 2).Length;
        }
    }
}
