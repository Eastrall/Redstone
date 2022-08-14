using Redstone.Common;
using Redstone.NBT.Tags;
using System;

namespace Redstone.Abstractions.Protocol;

/// <summary>
/// Provides a mechanism to read and write minecraft packets.
/// </summary>
public interface IMinecraftPacket
{
    /// <summary>
    /// Gets the packet id.
    /// </summary>
    int PacketId { get; }

    /// <summary>
    /// Gets the packet length.
    /// </summary>
    int PacketLength { get; }

    byte ReadByte();

    sbyte ReadSByte();

    char ReadChar();

    bool ReadBoolean();

    short ReadInt16();

    ushort ReadUInt16();

    int ReadInt32();

    uint ReadUInt32();

    long ReadInt64();

    ulong ReadUInt64();

    float ReadSingle();

    double ReadDouble();

    string ReadString();

    byte[] ReadBytes(int count);

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
    /// Reads 3 double values from the packet stream and transform it into a <see cref="Position"/> object.
    /// </summary>
    /// <remarks>
    /// The first double is the X coordinate.
    /// The second double is the Y coordinate.
    /// The third double is the Z coordinate.
    /// </remarks>
    /// <returns>Position.</returns>
    Position ReadAbsolutePosition();

    /// <summary>
    /// Reads a <see cref="NbtCompound"/> from the packet stream.
    /// </summary>
    /// <returns>Nbt Compound.</returns>
    NbtCompound ReadNbtCompound();

    void WriteByte(byte value);

    void WriteSByte(sbyte value);

    void WriteChar(char value);

    void WriteBoolean(bool value);

    void WriteInt16(short value);

    void WriteUInt16(ushort value);

    void WriteInt32(int value);

    void WriteUInt32(uint value);

    void WriteSingle(float value);

    void WriteDouble(double value);

    void WriteInt64(long value);

    void WriteUInt64(ulong value);

    void WriteString(string value);

    void WriteBytes(byte[] values);

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
    /// Writes the given angle as a single byte into the packet stream.
    /// </summary>
    /// <param name="angle">Angle.</param>
    void WriteAngle(float angle);

    /// <summary>
    /// Serializes the given object as JSON and writes the serialized content into the packet stream.
    /// </summary>
    /// <typeparam name="TObject">Objec type.</typeparam>
    /// <param name="object">Object to serialize.</param>
    void WriteJson<TObject>(TObject @object);

    /// <summary>
    /// Dumps the current packet stream into a file as a UTF-8 string.
    /// </summary>
    /// <param name="fileName">Dump output file name.</param>
    /// <param name="dumpMode">Dump mode.</param>
    void Dump(string fileName, PacketDumpMode dumpMode);
}
