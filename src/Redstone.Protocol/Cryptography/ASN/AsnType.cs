using System;
using System.Collections.Generic;
using System.Globalization;

namespace Redstone.Protocol.Cryptography.ASN
{
    /// <summary>
    /// ASN Format type.
    /// </summary>
    /// <remarks>
    /// Adapted from: https://github.com/jrnker/CSharp-easy-RSA-PEM/blob/master/CSharp-easy-RSA-PEM/CSharp-easy-RSA-PEM/AsnKeyBuilder.cs
    /// all credits to the original author.
    /// </remarks>
    internal class AsnType
    {
        private static readonly byte[] Zero = new byte[] { 0 };
        private static readonly byte[] Empty = new byte[] { };

        public AsnType(byte tag, byte octet)
        {
            Raw = false;
            _tag = new byte[] { tag };
            _octets = new byte[] { octet };
        }

        public AsnType(byte tag, byte[] octets)
        {
            Raw = false;
            _tag = new byte[] { tag };
            _octets = octets;
        }

        public AsnType(byte tag, byte[] length, byte[] octets)
        {
            Raw = true;
            _tag = new byte[] { tag };
            _length = length;
            _octets = octets;
        }

        private bool Raw { get; set; }

        private readonly byte[] _tag;
        public byte[] Tag => _tag ?? Empty;

        private byte[] _length;
        public byte[] Length => _length ?? Empty;

        private byte[] _octets;
        public byte[] Octets
        {
            get => _octets ?? Empty;
            set => _octets = value;
        }

        internal byte[] GetBytes()
        {
            if (true == Raw)
            {
                return Concatenate(new byte[][] { _tag, _length, _octets });
            }

            SetLength();

            if (0x05 == _tag[0])
            {
                return Concatenate(new byte[][] { _tag, _octets });
            }

            return Concatenate(new byte[][] { _tag, _length, _octets });
        }

        private void SetLength()
        {
            if (null == _octets)
            {
                _length = Zero;
                return;
            }

            if (0x05 == _tag[0])
            {
                _length = Empty;
                return;
            }

            byte[] length;

            if (_octets.Length < 0x80)
            {
                length = new byte[1];
                length[0] = (byte)_octets.Length;
            }
            else if (_octets.Length <= 0xFF)
            {
                length = new byte[2];
                length[0] = 0x81;
                length[1] = (byte)((_octets.Length & 0xFF));
            }


            else if (_octets.Length <= 0xFFFF)
            {
                length = new byte[3];
                length[0] = 0x82;
                length[1] = (byte)((_octets.Length & 0xFF00) >> 8);
                length[2] = (byte)((_octets.Length & 0xFF));
            }

            else if (_octets.Length <= 0xFFFFFF)
            {
                length = new byte[4];
                length[0] = 0x83;
                length[1] = (byte)((_octets.Length & 0xFF0000) >> 16);
                length[2] = (byte)((_octets.Length & 0xFF00) >> 8);
                length[3] = (byte)((_octets.Length & 0xFF));
            }
            else
            {
                length = new byte[5];
                length[0] = 0x84;
                length[1] = (byte)((_octets.Length & 0xFF000000) >> 24);
                length[2] = (byte)((_octets.Length & 0xFF0000) >> 16);
                length[3] = (byte)((_octets.Length & 0xFF00) >> 8);
                length[4] = (byte)((_octets.Length & 0xFF));
            }

            _length = length;
        }

        public static AsnType CreateOctetString(byte[] value)
        {
            if (IsEmpty(value))
            {
                return new AsnType(0x04, EMPTY);
            }

            return new AsnType(0x04, value);
        }

        public static AsnType CreateOctetString(AsnType value)
        {
            if (IsEmpty(value))
            {
                return new AsnType(0x04, 0x00);
            }

            return new AsnType(0x04, value.GetBytes());
        }

        public static AsnType CreateOctetString(AsnType[] values)
        {
            if (IsEmpty(values))
            {
                return new AsnType(0x04, 0x00);
            }

            return new AsnType(0x04, Concatenate(values));
        }

        public static AsnType CreateOctetString(String value)
        {
            if (IsEmpty(value))
            { return CreateOctetString(EMPTY); }

            int len = (value.Length + 255) / 256;

            List<byte> octets = new List<byte>();
            for (int i = 0; i < len; i++)
            {
                String s = value.Substring(i * 2, 2);
                byte b = 0x00;

                try
                { b = Convert.ToByte(s, 16); }
                catch (FormatException /*e*/) { break; }
                catch (OverflowException /*e*/) { break; }

                octets.Add(b);
            }

            return CreateOctetString(octets.ToArray());
        }

        public static AsnType CreateBitString(byte[] octets)
        {
            return CreateBitString(octets, 0);
        }

        public static AsnType CreateBitString(byte[] octets, uint unusedBits)
        {
            if (IsEmpty(octets))
            {
                return new AsnType(0x03, EMPTY);
            }

            if (!(unusedBits < 8))
            { throw new ArgumentException("Unused bits must be less than 8."); }

            byte[] b = Concatenate(new byte[] { (byte)unusedBits }, octets);
            return new AsnType(0x03, b);
        }

        public static AsnType CreateBitString(AsnType value)
        {
            if (IsEmpty(value))
            { return new AsnType(0x03, EMPTY); }

            return CreateBitString(value.GetBytes(), 0x00);
        }

        public static AsnType CreateBitString(AsnType[] values)
        {
            if (IsEmpty(values))
            { return new AsnType(0x03, EMPTY); }

            return CreateBitString(Concatenate(values), 0x00);
        }

        public static AsnType CreateBitString(String value)
        {
            if (IsEmpty(value))
            { return CreateBitString(EMPTY); }

            int lstrlen = value.Length;
            int unusedBits = 8 - (lstrlen % 8);
            if (8 == unusedBits) { unusedBits = 0; }

            for (int i = 0; i < unusedBits; i++)
            { value += "0"; }

            int loctlen = (lstrlen + 7) / 8;

            List<byte> octets = new List<byte>();
            for (int i = 0; i < loctlen; i++)
            {
                String s = value.Substring(i * 8, 8);
                byte b = 0x00;

                try
                { b = Convert.ToByte(s, 2); }

                catch (FormatException /*e*/) { unusedBits = 0; break; }
                catch (OverflowException /*e*/) { unusedBits = 0; break; }

                octets.Add(b);
            }

            return CreateBitString(octets.ToArray(), (uint)unusedBits);
        }

        public static byte[] ZERO = new byte[] { 0 };
        public static byte[] EMPTY = new byte[] { };

        public static bool IsZero(byte[] octets)
        {
            if (IsEmpty(octets))
            { return false; }

            bool allZeros = true;
            for (int i = 0; i < octets.Length; i++)
            {
                if (0 != octets[i])
                { allZeros = false; break; }
            }
            return allZeros;
        }

        public static bool IsEmpty(byte[] octets)
        {
            if (null == octets || 0 == octets.Length)
            { return true; }

            return false;
        }

        public static bool IsEmpty(String s)
        {
            if (null == s || 0 == s.Length)
            { return true; }

            return false;
        }

        public static bool IsEmpty(String[] strings)
        {
            if (null == strings || 0 == strings.Length)
                return true;

            return false;
        }

        public static bool IsEmpty(AsnType value)
        {
            if (null == value)
            { return true; }

            return false;
        }

        public static bool IsEmpty(AsnType[] values)
        {
            if (null == values || 0 == values.Length)
                return true;

            return false;
        }

        public static bool IsEmpty(byte[][] arrays)
        {
            if (null == arrays || 0 == arrays.Length)
                return true;

            return false;
        }

        public static AsnType CreateInteger(byte[] value)
        {
            if (IsEmpty(value))
            { return CreateInteger(ZERO); }

            return new AsnType(0x02, value);
        }

        public static AsnType CreateNull()
        {
            return new AsnType(0x05, new byte[] { 0x00 });
        }

        public static byte[] Duplicate(byte[] b)
        {
            if (IsEmpty(b))
            { return EMPTY; }

            byte[] d = new byte[b.Length];
            Array.Copy(b, d, b.Length);

            return d;
        }

        public static AsnType CreateIntegerPos(byte[] value)
        {
            byte[] i = null, d = Duplicate(value);

            if (IsEmpty(d)) { d = ZERO; }

            if (d.Length > 0 && d[0] > 0x7F)
            {
                i = new byte[d.Length + 1];
                i[0] = 0x00;
                Array.Copy(d, 0, i, 1, value.Length);
            }
            else
            {
                i = d;
            }

            return CreateInteger(i);
        }

        public static byte[] Concatenate(AsnType[] values)
        {
            if (IsEmpty(values))
                return new byte[] { };

            int length = 0;
            foreach (AsnType t in values)
            {
                if (null != t)
                { length += t.GetBytes().Length; }
            }

            byte[] cated = new byte[length];

            int current = 0;
            foreach (AsnType t in values)
            {
                if (null != t)
                {
                    byte[] b = t.GetBytes();

                    Array.Copy(b, 0, cated, current, b.Length);
                    current += b.Length;
                }
            }

            return cated;
        }

        public static byte[] Concatenate(byte[] first, byte[] second)
        {
            return Concatenate(new byte[][] { first, second });
        }

        public static byte[] Concatenate(byte[][] values)
        {
            if (IsEmpty(values))
                return new byte[] { };

            int length = 0;
            foreach (byte[] b in values)
            {
                if (null != b)
                { length += b.Length; }
            }

            byte[] cated = new byte[length];

            int current = 0;
            foreach (byte[] b in values)
            {
                if (null != b)
                {
                    Array.Copy(b, 0, cated, current, b.Length);
                    current += b.Length;
                }
            }

            return cated;
        }

        public static AsnType CreateSequence(AsnType[] values)
        {

            if (IsEmpty(values))
            { throw new ArgumentException("A sequence requires at least one value."); }

            return new AsnType((0x10 | 0x20), Concatenate(values));
        }

        public static AsnType CreateOid(String value)
        {
            if (IsEmpty(value))
                return null;

            String[] tokens = value.Split(new Char[] { ' ', '.' });

            if (IsEmpty(tokens))
                return null;

            UInt64 a = 0;

            List<UInt64> arcs = new List<UInt64>();

            foreach (String t in tokens)
            {
                if (t.Length == 0) { break; }

                try { a = Convert.ToUInt64(t, CultureInfo.InvariantCulture); }
                catch (FormatException /*e*/) { break; }
                catch (OverflowException /*e*/) { break; }

                arcs.Add(a);
            }

            if (0 == arcs.Count)
                return null;

            List<byte> octets = new List<byte>();

            if (arcs.Count >= 1) { a = arcs[0] * 40; }
            if (arcs.Count >= 2) { a += arcs[1]; }
            octets.Add((byte)(a));

            for (int i = 2; i < arcs.Count; i++)
            {
                List<byte> temp = new List<byte>();

                UInt64 arc = arcs[i];

                do
                {
                    temp.Add((byte)(0x80 | (arc & 0x7F)));
                    arc >>= 7;
                } while (0 != arc);

                byte[] t = temp.ToArray();

                t[0] = (byte)(0x7F & t[0]);

                Array.Reverse(t);

                foreach (byte b in t)
                { octets.Add(b); }
            }

            return CreateOid(octets.ToArray());
        }

        public static AsnType CreateOid(byte[] value)
        {
            if (IsEmpty(value))
            { return null; }

            return new AsnType(0x06, value);
        }

        public static byte[] Compliment1s(byte[] value)
        {
            if (IsEmpty(value))
            { return EMPTY; }

            byte[] c = Duplicate(value);

            for (int i = c.Length - 1; i >= 0; i--)
            {
                c[i] = (byte)~c[i];
            }

            return c;
        }
    }
}
