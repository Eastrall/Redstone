using System;

namespace Redstone.NBT.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation is attempted on an NbtReader that cannot recover from a previous parsing error.
    /// </summary>
    [Serializable]
    public sealed class InvalidReaderStateException : InvalidOperationException
    {
        /// <summary>
        /// Creates a new <see cref="InvalidReaderStateException"/> exception instance with a message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        internal InvalidReaderStateException(string message)
            : base(message)
        {
        }
    }
}
