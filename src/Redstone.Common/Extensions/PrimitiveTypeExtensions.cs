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

        /// <summary>
        /// Converts a randian angle to degree.
        /// </summary>
        /// <param name="radians">Radian angle value.</param>
        /// <returns>The radian angle converted to a degree angle.</returns>
        public static float ToDegree(this float radians) => (float)(radians * (180.0f / Math.PI));

        /// <summary>
        /// Converts a degree angle to radian.
        /// </summary>
        /// <param name="degree">Degree angle value.</param>
        /// <returns>Degree angle converted to a radian angle.</returns>
        public static float ToRadian(this float degree) => (float)(degree * (Math.PI / 180.0f));

        /// <summary>
        /// Gives the percentage of a value.
        /// </summary>
        /// <param name="value">Reference value</param>
        /// <param name="percents">Percentage wanted</param>
        /// <returns></returns>
        public static int Percentage(this int value, int percents) => (value * percents) / 100;
    }
}
