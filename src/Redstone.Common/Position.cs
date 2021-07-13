using Redstone.Common.Extensions;
using System;
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
        public static Position Zero => new();

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
        /// Gets the vector length.
        /// </summary>
        public float Length => (float)Math.Sqrt(SquaredLength);

        /// <summary>
        /// Gets the vector squared length.
        /// </summary>
        public float SquaredLength => (X * X) + (Y * Y) + (Z * Z);

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

        /// <summary>
        /// Creates a new <see cref="Position"/> with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        public Position(double x, double y, double z)
            : this((float)x, (float)y, (float)z)
        {
        }

        /// <summary>
        /// Checks if the current position is in a given range.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public bool IsInRange(Position otherPosition, float range)
        {
            Position distance = this - otherPosition;
            distance.Y = 0;

            return distance.SquaredLength <= range * range;
        }
        /// <summary>
        /// Normalize the Position.
        /// </summary>
        /// <returns></returns>
        public Position Normalize()
        {
            var sqLength = SquaredLength;

            if (sqLength <= 0)
            {
                throw new InvalidOperationException("Cannot normalize a Position of zero length.");
            }

            return this / (float)Math.Sqrt(sqLength);
        }

        /// <summary>
        /// Clones this Position3 instance.
        /// </summary>
        /// <returns></returns>
        public Position Clone() => new(X, Y, Z);

        /// <summary>
        /// Reset to 0 this Position3.
        /// </summary>
        public void Reset()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Copies the other Position values into the current Position.
        /// </summary>
        /// <param name="otherPosition">Other Position.</param>
        public void Copy(Position otherPosition)
        {
            X = otherPosition.X;
            Y = otherPosition.Y;
            Z = otherPosition.Z;
        }

        /// <summary>
        /// Check if the Position3 is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsZero() => SquaredLength <= 0;

        /// <summary>
        /// Returns a Position3 under string format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{X},{Y},{Z}";

        /// <summary>
        /// Returns the HashCode for this Position
        /// </summary>
        /// <returns> 
        /// int - the HashCode for this Position
        /// </returns> 
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();

        /// <summary>
        /// Compares two Positions.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => this == (Position)obj;

        /// <summary>
        /// Add two Position.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Subtract two Position.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Multiplies two Position.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Position operator *(Position a, Position b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        /// <summary>
        /// Multiplies a Position and a float value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Position operator *(Position a, float b) => new(a.X * b, a.Y * b, a.Z * b);

        /// <summary>
        /// Divides two Position.
        /// </summary>
        /// <remarks>
        /// Be careful with the <see cref="DivideByZeroException"/>.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Position operator /(Position a, Position b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        /// <summary>
        /// Divides a Position by a scalar number.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Position operator /(Position a, float b) => new(a.X / b, a.Y / b, a.Z / b);

        /// <summary>
        /// Compares two Position.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Position a, Position b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return Math.Ceiling(a.X - b.X) < 0.01 && Math.Ceiling(a.Y - b.Y) < 0.01 && Math.Ceiling(a.Z - b.Z) < 0.01;
        }

        /// <summary>
        /// Compares two Position.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Get the angle between two Positions.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AngleBetween(Position a, Position b)
        {
            var dist = b - a;
            float angle = (float)Math.Atan2(dist.X, -dist.Z);

            angle = angle.ToDegree();

            if (angle < 0)
            {
                angle += 360;
            }
            else if (angle >= 360)
            {
                angle -= 360;
            }

            return angle;
        }

        /// <summary>
        /// Gets the 2D distance between two Positions.
        /// </summary>
        /// <param name="from">Origin Position.</param>
        /// <param name="to">Target Position.</param>
        /// <returns>Distance</returns>
        public static float Distance2D(Position from, Position to)
        {
            float x = from.X - to.X;
            float z = from.Z - to.Z;

            return (float)Math.Sqrt(x * x + z * z);
        }

        /// <summary>
        /// Gets the 3D distance between two Positions.
        /// </summary>
        /// <param name="from">Origin Position.</param>
        /// <param name="to">Target Position.</param>
        /// <returns>Distance</returns>
        public static float Distance3D(Position from, Position to)
        {
            float x = from.X - to.X;
            float y = from.Y - to.Y;
            float z = from.Z - to.Z;

            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Compares two <see cref="Position"/> objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Position other) => this == other;
    }
}
