using System;

namespace Redstone.NBT.Exceptions
{
    /// <summary> 
    /// Exception thrown when a format violation is detected while parsing or serializing an NBT file. </summary>
    [Serializable]
    public sealed class NbtFormatException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="NbtFormatException"/> with a message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        internal NbtFormatException(string message)
            : base(message)
        {
        }
    }
}
