using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.Common.Collections
{
    /// <summary>
    /// Provides a mechanism to store multiple numeric values inside a single long.
    /// </summary>
    /// <remarks>
    /// Based on https://github.com/PrismarineJS/prismarine-chunk/blob/master/src/pc/common/BitArrayNoSpan.js
    /// </remarks>
    public class CompactedLongArray : IEnumerable<long>
    {
        //private readonly int _valuesPerLong;
        //private readonly long _valueMask;
        //private readonly long[] _storage;

        /// <summary>
        /// Gets the number of bits per entry.
        /// </summary>
        public byte BitsPerEntry { get; }

        /// <summary>
        /// Gets the array capacity in bits.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Gets the array capacity in bits.
        /// </summary>
        public int Length => _storage.Length;


        public long[] Storage => _storage;

        ///// <summary>
        ///// Gets the value at the given index.
        ///// </summary>
        ///// <param name="index">Array index.</param>
        ///// <returns>Value</returns>
        //public long this[int index]
        //{
        //    get => Get(index);
        //    set => Set(index, value);
        //}

        ///// <summary>
        ///// Creates a new <see cref="CompactedLongArray"/> instance.
        ///// </summary>
        ///// <param name="bitsPerEntry">Number of bits per entry.</param>
        ///// <param name="capacity">Maximum array capacity.</param>
        //public CompactedLongArray(byte bitsPerEntry, int capacity)
        //{
        //    BitsPerEntry = bitsPerEntry;
        //    Capacity = capacity;
        //    _valueMask = (1 << BitsPerEntry) - 1;
        //    _valuesPerLong = (int)Math.Floor(64m / BitsPerEntry);
        //    int arrayLength = (int)Math.Ceiling((decimal)(capacity / _valuesPerLong));

        //    _storage = new long[arrayLength * 2];
        //}

        ///// <summary>
        ///// Gets the value at the given index.
        ///// </summary>
        ///// <param name="index">Array index.</param>
        ///// <returns>Value.</returns>
        //public long Get(int index)
        //{
        //    int startLongIndex = (int)Math.Floor(((float)index / (float)_valuesPerLong));
        //    int indexInLong = (index - startLongIndex * _valuesPerLong) * BitsPerEntry;

        //    if (indexInLong >= 32)
        //    {
        //        var indexInStartLong = indexInLong - 32;
        //        var startLong = _storage[startLongIndex * 2 + 1];

        //        return (long)((ulong)startLong >> indexInStartLong) & _valueMask;
        //    }

        //    var startLong2 = _storage[startLongIndex * 2];
        //    var indexInStartLong2 = indexInLong;
        //    var result = (long)((ulong)(startLong2 >> indexInStartLong2));
        //    var endBitOffset = indexInStartLong2 + BitsPerEntry;

        //    if (endBitOffset > 32)
        //    {
        //        // Value stretches across multiple longs
        //        long endLong = _storage[startLongIndex * 2 + 1];
        //        result |= endLong << (32 - indexInStartLong2);
        //    }
        //    return result & _valueMask;
        //}

        ///// <summary>
        ///// Sets a value at the given index.
        ///// </summary>
        ///// <param name="index">Array index.</param>
        ///// <param name="value">Value to store.</param>
        //public void Set(int index, long value)
        //{
        //    int startLongIndex = (int)Math.Floor((decimal)(index / _valuesPerLong));
        //    int indexInLong = (index - startLongIndex * _valuesPerLong) * BitsPerEntry;

        //    int indexInStartLong;
        //    ulong longValue;

        //    if (indexInLong >= 32)
        //    {
        //        indexInStartLong = indexInLong - 32;
        //        longValue = (ulong)((_storage[startLongIndex * 2 + 1] &
        //            ~(_valueMask << indexInStartLong)) |
        //            ((value & _valueMask) << indexInStartLong));

        //        _storage[startLongIndex * 2 + 1] = (long)(longValue);
        //        return;
        //    }

        //    indexInStartLong = indexInLong;

        //    long storedValue = _storage[startLongIndex];

        //    longValue = (ulong)((storedValue & ~(_valueMask << indexInStartLong)) | ((value & _valueMask) << indexInStartLong));

        //    // Clear bits of this value first
        //    _storage[startLongIndex * 2] = (long)longValue;

        //    var endBitOffset = indexInStartLong + BitsPerEntry;
        //    if (endBitOffset > 32)
        //    {
        //        longValue = (ulong)((_storage[startLongIndex * 2 + 1] &
        //            ~((1 << (endBitOffset - 32)) - 1)) |
        //          (value >> (32 - indexInStartLong)));

        //        // Value stretches across multiple longs
        //        _storage[startLongIndex * 2 + 1] = (long)longValue;
        //    }
        //}

        public IEnumerator<long> GetEnumerator()
        {
            return _storage.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.AsEnumerable().GetEnumerator();
        }

        // From ObsidianMC for testing purposes only
        // Thank you to them

        private static readonly int[] yes = new int[] { -1, -1, 0, int.MinValue, 0, 0, 1431655765, 1431655765, 0, int.MinValue, 0, 1, 858993459, 858993459, 0, 715827882, 715827882, 0, 613566756, 613566756, 0, int.MinValue, 0, 2, 477218588, 477218588, 0, 429496729, 429496729, 0, 390451572, 390451572, 0, 357913941, 357913941, 0, 330382099, 330382099, 0, 306783378, 306783378, 0, 286331153, 286331153, 0, int.MinValue, 0, 3, 252645135, 252645135, 0, 238609294, 238609294, 0, 226050910, 226050910, 0, 214748364, 214748364, 0, 204522252, 204522252, 0, 195225786, 195225786, 0, 186737708, 186737708, 0, 178956970, 178956970, 0, 171798691, 171798691, 0, 165191049, 165191049, 0, 159072862, 159072862, 0, 153391689, 153391689, 0, 148102320, 148102320, 0, 143165576, 143165576, 0, 138547332, 138547332, 0, int.MinValue, 0, 4, 130150524, 130150524, 0, 126322567, 126322567, 0, 122713351, 122713351, 0, 119304647, 119304647, 0, 116080197, 116080197, 0, 113025455, 113025455, 0, 110127366, 110127366, 0, 107374182, 107374182, 0, 104755299, 104755299, 0, 102261126, 102261126, 0, 99882960, 99882960, 0, 97612893, 97612893, 0, 95443717, 95443717, 0, 93368854, 93368854, 0, 91382282, 91382282, 0, 89478485, 89478485, 0, 87652393, 87652393, 0, 85899345, 85899345, 0, 84215045, 84215045, 0, 82595524, 82595524, 0, 81037118, 81037118, 0, 79536431, 79536431, 0, 78090314, 78090314, 0, 76695844, 76695844, 0, 75350303, 75350303, 0, 74051160, 74051160, 0, 72796055, 72796055, 0, 71582788, 71582788, 0, 70409299, 70409299, 0, 69273666, 69273666, 0, 68174084, 68174084, 0, int.MinValue, 0, 5 };

        private long[] _storage;
        //private readonly int bitsPerEntry;
        private readonly long _valueMask;

        private int specialIndex;

        private readonly int magicNumber;

        public CompactedLongArray(byte bitsPerEntryIn, int arraySizeIn)
        {
            BitsPerEntry = bitsPerEntryIn;
            _valueMask = (1L << bitsPerEntryIn) - 1L;
            magicNumber = (char)(64 / bitsPerEntryIn);

            specialIndex = 3 * (magicNumber - 1);

            int storageSize = (arraySizeIn + magicNumber - 1) / magicNumber;

            _storage = new long[storageSize];
        }

        public long this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public long Get(int index)
        {
            int idx = GetIndex(index);
            long storedValue = _storage[idx];
            int shift = (index - idx * magicNumber) * BitsPerEntry;

            return (int)(storedValue >> shift & _valueMask);
        }

        public void Set(int index, long value)
        {
            int idx = GetIndex(index);
            long storedValue = _storage[idx];
            int shift = (index - idx * magicNumber) * BitsPerEntry;

            _storage[idx] = storedValue & ~(_valueMask << shift) | (value & _valueMask) << shift;
        }

        private int GetIndex(int index)
        {
            long i = yes[specialIndex] & 0xffffffffL;
            long j = yes[specialIndex + 1] & 0xffffffffL;
            return (int)(index * i + j >> 32 >> yes[specialIndex + 2]);
        }
    }
}
