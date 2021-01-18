using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redstone.Common.Collections
{
    public class CompactedLongArray
    {
        private readonly int _valuesPerLong;
        private readonly long _valueMask;

        private long[] _storage;

        /// <summary>
        /// Gets the number of bits per entry.
        /// </summary>
        public byte BitsPerEntry { get; }

        /// <summary>
        /// Gets the array capacity in bits.
        /// </summary>
        public int Length { get; }

        public long this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public CompactedLongArray(byte bitsPerEntry, int capacity)
        {
            BitsPerEntry = bitsPerEntry;
            Length = capacity;
            _valueMask = (1 << BitsPerEntry) - 1;
            _valuesPerLong = (int)Math.Floor(64m / BitsPerEntry);
            int arrayLength = (int)Math.Ceiling((decimal)(capacity / _valuesPerLong));

            _storage = new long[arrayLength * 2];
        }

        public long Get(int index)
        {
            int startLongIndex = (int)Math.Floor((decimal)(index / _valuesPerLong));
            int indexInLong = (index - startLongIndex * _valuesPerLong) * BitsPerEntry;

            int indexInStartLong;
            ulong startLong;

            if (indexInLong >= 32)
            {
                indexInStartLong = indexInLong - 32;
                startLong = (ulong)_storage[startLongIndex * 2 + 1];

                return ((long)startLong >> indexInStartLong) & _valueMask;
            }

            indexInStartLong = indexInLong;
            startLong = (ulong)_storage[startLongIndex * 2];

            var result = (long)startLong >> indexInStartLong;
            var endBitOffset = indexInStartLong + BitsPerEntry;

            if (endBitOffset > 32)
            {
                // Value stretches across multiple longs
                var endLong = (ulong)_storage[startLongIndex * 2 + 1];
                result |= (long)endLong << (32 - indexInStartLong);
            }
            return result & _valueMask;
        }

        public void Set(int index, long value)
        {
            int startLongIndex = (int)Math.Floor((decimal)(index / _valuesPerLong));
            int indexInLong = (index - startLongIndex * _valuesPerLong) * BitsPerEntry;

            int indexInStartLong;
            ulong longValue;

            if (indexInLong >= 32)
            {
                indexInStartLong = indexInLong - 32;
                longValue = (ulong)((_storage[startLongIndex * 2 + 1] &
                    ~(_valueMask << indexInStartLong)) |
                    ((value & _valueMask) << indexInStartLong));

                _storage[startLongIndex * 2 + 1] = (long)(longValue >> 0);
                return;
            }

            indexInStartLong = indexInLong;
            longValue = (ulong)((_storage[startLongIndex * 2] & 
                ~(_valueMask << indexInStartLong)) |
                ((value & _valueMask) << indexInStartLong));

            // Clear bits of this value first
            _storage[startLongIndex * 2] = (long)longValue;

            var endBitOffset = indexInStartLong + BitsPerEntry;
            if (endBitOffset > 32)
            {
                longValue = (ulong)((_storage[startLongIndex * 2 + 1] &
                    ~((1 << (endBitOffset - 32)) - 1)) |
                  (value >> (32 - indexInStartLong)));

                // Value stretches across multiple longs
                _storage[startLongIndex * 2 + 1] = (long)longValue;
            }
        }
    }
}
