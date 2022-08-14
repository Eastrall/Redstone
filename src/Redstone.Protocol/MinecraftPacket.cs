using Redstone.Abstractions.Protocol;
using Redstone.Common;
using Redstone.Common.IO;
using Redstone.Common.Serialization;
using Redstone.Common.Utilities;
using Redstone.NBT;
using Redstone.NBT.Tags;
using System;
using System.IO;
using System.Text;

namespace Redstone.Protocol
{
    /// <summary>
    /// Minecraft packet implementation.
    /// </summary>
    public class MinecraftPacket : MinecraftStream, IMinecraftPacket
    {
        protected override bool ReverseIfLittleEndian => true;

        public byte[] BaseBuffer => base.Buffer;

        public int PacketId { get; }

        public int PacketLength => VariableStorageUtilities.GetVarInt32Length(PacketId) + (int)Length;

        /// <summary>
        /// Gets the final <see cref="MinecraftPacket"/> buffer including the packet size.
        /// </summary>
        //public override byte[] Buffer
        //{
        //    get
        //    {
        //        using var stream = new MinecraftStream();

        //        stream.WriteVarInt32(PacketLength);
        //        stream.WriteVarInt32(PacketId);
        //        stream.WriteBytes(BaseBuffer);

        //        return stream.Buffer;
        //    }
        //}

        /// <summary>
        /// Creates a new empty <see cref="MinecraftPacket"/> stream in write-only mode.
        /// </summary>
        public MinecraftPacket()
        {
        }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in write-only mode with a given packet id as an integer.
        /// </summary>
        /// <param name="packetId">Packet Id.</param>
        public MinecraftPacket(int packetId)
        {
            PacketId = packetId;
            //WriteVarInt32(PacketId);
        }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in write-only mode with a given packet id as an enum value.
        /// </summary>
        /// <param name="packetId">Packet Id.</param>
        public MinecraftPacket(Enum packetId)
            : this(Convert.ToInt32(packetId))
        {
        }

        /// <summary>
        /// Creates a new <see cref="MinecraftPacket"/> in read-only mode.
        /// </summary>
        /// <param name="buffer">Packet buffer data.</param>
        public MinecraftPacket(int packetId, byte[] buffer) 
            : base(buffer)
        {
            PacketId = packetId;
        }

        public Position ReadPosition()
        {
            ulong positionAsLong = ReadUInt64();
            var position = new Position
            {
                X = positionAsLong >> 38,
                Y = positionAsLong & 0xFFF,
                Z = positionAsLong << 26 >> 38
            };

            return position;
        }

        public Position ReadAbsolutePosition()
        {
            var x = ReadDouble();
            var y = ReadDouble();
            var z = ReadDouble();

            return new Position(x, y, z);
        }

        public NbtCompound ReadNbtCompound()
        {
            var reader = new NbtReader(this);

            return reader.IsCompound ? reader.ReadAsTag() as NbtCompound : new NbtCompound();
        }

        public void WritePosition(Position position)
        {
            var x = (((int)position.X & 0x3FFFFFF) << 38);
            var z = (((int)position.Z & 0x3FFFFFF) << 12);
            var y = ((int)position.Y & 0xFFF);

            WriteInt64(x | z | y);
        }

        public void WriteAngle(float angle)
        {
            WriteByte((byte)(angle * 256f / 360f));
        }

        public void WriteJson<TObject>(TObject @object) => WriteString(JsonSerializer.Serialize(@object));

        public void Dump(string fileName, PacketDumpMode dumpMode)
        {
            var dumpContent = dumpMode switch
            {
                PacketDumpMode.Default => string.Join(Environment.NewLine, BaseBuffer),
                PacketDumpMode.UTF8String => Encoding.UTF8.GetString(BaseBuffer),
                _ => null,
            };

            if (!string.IsNullOrEmpty(dumpContent))
            {
                File.WriteAllText(fileName, dumpContent);
            }
        }
    }
}
