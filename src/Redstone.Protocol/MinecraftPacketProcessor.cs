using LiteNetwork.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Protocol
{
    public class MinecraftPacketProcessor : LitePacketProcessor
    {
        public override bool ReadHeader(LiteDataToken token, byte[] buffer, int bytesTransfered)
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

        public override int GetMessageLength(byte[] buffer)
        {
            int length = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                length |= buffer[i];
            }

            return length;
        }

        public override byte[] AppendHeader(byte[] buffer)
        {
            // Nothing to append here. We already do the packet header append process in
            // the MinecraftUser.Send() method.

            return buffer;
        }
    }
}
