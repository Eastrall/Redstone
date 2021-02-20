using System.Diagnostics;

namespace Redstone.Common
{
    /// <summary>
    /// Provides a data structure that represents a 3D position in the world.
    /// </summary>
    [DebuggerDisplay("{X}/{Y}/{Z}")]
    public class Position
    {
        /// <summary>
        /// Gets the zero position.
        /// </summary>
        public static Position Zero => new Position();

        /// <summary>
        /// Gets or sets the X coordinate position.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate position.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the Z coordinate position.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Creates a new empty <see cref="Position"/>.
        /// </summary>
        public Position()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Position"/> with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
