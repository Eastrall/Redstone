using System;
using System.IO;

namespace Redstone.Common.Utilities;

public static class VariableStorageUtilities
{
    public static int ReadVarInt32(byte[] buffer)
    {
        using MemoryStream stream = new(buffer);

        return ReadVarInt32(stream);
    }

    public static int ReadVarInt32(Stream stream)
    {
        int numRead = 0;
        int result = 0;
        byte read;

        do
        {
            read = (byte)stream.ReadByte();
            int value = (read & 0b01111111);
            result |= (value << (7 * numRead));

            numRead++;
            if (numRead > 5)
            {
                throw new InvalidOperationException("VarInt32 is too big.");
            }
        } while ((read & 0b10000000) != 0);

        return result;
    }

    public static byte[] GetVarInt32Buffer(int value)
    {
        var buffer = new byte[GetVarInt32Length(value)];
        var valueToWrite = (uint)value;
        int index = 0;

        do
        {
            var temp = (byte)(valueToWrite & 127);

            valueToWrite >>= 7;

            if (valueToWrite != 0)
            {
                temp |= sbyte.MaxValue + 1;
            }

            buffer[index++] = temp;
            //WriteByte(temp);
        } while (valueToWrite != 0);

        return buffer;
    }

    public static int GetVarInt32Length(int value)
    {
        int amount = 0;

        do
        {
            value >>= 7;
            amount++;
        } while (value != 0);

        return amount;
    }
}
