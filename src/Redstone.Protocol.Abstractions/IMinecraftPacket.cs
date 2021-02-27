using LiteNetwork.Protocol.Abstractions;
using Redstone.Common;
using System;

namespace Redstone.Protocol.Abstractions
{
    /// <summary>
    /// Provides a mechanism to read and write minecraft packets.
    /// </summary>
    public interface IMinecraftPacket : ILitePacketStream
    {
        /// <summary>
        /// Gets the packet id.
        /// </summary>
        int PacketId { get; }

        /// <summary>
        /// Reads an Univeral Unique IDentifier value from the packet stream.
        /// </summary>
        /// <returns>UUID</returns>
        /// <remarks>
        /// A UUID is a 128 bit signed integer.
        /// </remarks>
        Guid ReadUUID();

        /// <summary>
        /// Reads a 4 byte variable integer value from the packet stream.
        /// </summary>
        /// <returns>Integer value.</returns>
        int ReadVarInt32();

        /// <summary>
        /// Reads a 8 bytes variable numeric value from the packet stream.
        /// </summary>
        /// <returns>Long numeric value.</returns>
        long ReadVarInt64();

        /// <summary>
        /// Reads an integer from the packet stream and transforms it into a <see cref="Position"/> object.
        /// </summary>
        /// <returns>Position.</returns>
        Position ReadPosition();

        /// <summary>
        /// Writes an Universal Unique IDentifier into the packet stream.
        /// </summary>
        /// <param name="value"></param>
        void WriteUUID(Guid value);

        /// <summary>
        /// Writes a 4 byte variable numeric value into the packet stream.
        /// </summary>
        /// <param name="value">Numeric integer value.</param>
        void WriteVarInt32(int value);

        /// <summary>
        /// Writes a 8 byte variable numeric value into the packet stream.
        /// </summary>
        /// <param name="value">Numeric long value.</param>
        void WriteVarInt64(long value);

        /// <summary>
        /// Writes a the given position as an integer into the packet stream.
        /// </summary>
        /// <param name="position">Position.</param>
        void WritePosition(Position position);

        /// <summary>
        /// Dumps the current packet stream into a file as a UTF-8 string.
        /// </summary>
        /// <param name="fileName">Dump output file name.</param>
        /// <param name="dumpMode">Dump mode.</param>
        void Dump(string fileName, PacketDumpMode dumpMode);
    }
}
