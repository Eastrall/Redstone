using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Protocol
{
    public class MinecraftPacketProcessor : LitePacketProcessor
    {
        public override ILitePacketStream CreatePacket(byte[] buffer)
        {
            int cursor = 0;
            int numRead = 0;
            int packetHeader = 0;
            byte read;

            do
            {
                read = buffer[cursor++];
                int value = (read & 0b01111111);
                packetHeader |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new InvalidOperationException("VarInt32 is too big.");
                }
            } while ((read & 0b10000000) != 0);

            return new MinecraftPacket(packetHeader, buffer.Skip(numRead).ToArray());
        }

        public override int GetMessageLength(byte[] buffer)
        {
            int length = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                length |= buffer[i];
            }

            return length;
        }

        public override bool ParseHeader(LiteDataToken token, byte[] buffer, int bytesTransfered)
        {
            int bufferRemainingBytes = bytesTransfered - token.DataStartOffset;

            if (bufferRemainingBytes <= 0)
            {
                return false;
            }

            var data = new List<byte>();
            int numRead = 0;
            byte read;

            do
            {
                read = buffer[token.DataStartOffset++];
                int value = (read & 0b01111111);

                data.Add((byte)(value << (7 * numRead)));

                numRead++;
                if (numRead > 5)
                {
                    throw new InvalidOperationException("Failed to read a variable length integer.");
                }
            } while ((read & 0b10000000) != 0);

            token.HeaderData = token.HeaderData is null ? data.ToArray() : token.HeaderData.Concat(data).ToArray();

            return true;
        }
    }
}
