namespace Redstone.Common.Collections
{
    /// <summary>
    /// Provides a mechanism to store multiple numeric values inside a single long.
    /// </summary>
    /// <remarks>
    /// Based on https://github.com/Minestom/Minestom/blob/master/src/main/java/net/minestom/server/utils/Utils.java
    /// and adapted for Redstone needs. All credits to the original authors.
    /// </remarks>
    public class CompactedLongArray
    {
        private static readonly int[] CompactedMagicNumbers = {
            -1, -1, 0, int.MinValue, 0, 0, 1431655765, 1431655765, 0, int.MinValue,
            0, 1, 858993459, 858993459, 0, 715827882, 715827882, 0, 613566756, 613566756,
            0, int.MinValue, 0, 2, 477218588, 477218588, 0, 429496729, 429496729, 0,
            390451572, 390451572, 0, 357913941, 357913941, 0, 330382099, 330382099, 0, 306783378,
            306783378, 0, 286331153, 286331153, 0, int.MinValue, 0, 3, 252645135, 252645135,
            0, 238609294, 238609294, 0, 226050910, 226050910, 0, 214748364, 214748364, 0,
            204522252, 204522252, 0, 195225786, 195225786, 0, 186737708, 186737708, 0, 178956970,
            178956970, 0, 171798691, 171798691, 0, 165191049, 165191049, 0, 159072862, 159072862,
            0, 153391689, 153391689, 0, 148102320, 148102320, 0, 143165576, 143165576, 0,
            138547332, 138547332, 0, int.MinValue, 0, 4, 130150524, 130150524, 0, 126322567,
            126322567, 0, 122713351, 122713351, 0, 119304647, 119304647, 0, 116080197, 116080197,
            0, 113025455, 113025455, 0, 110127366, 110127366, 0, 107374182, 107374182, 0,
            104755299, 104755299, 0, 102261126, 102261126, 0, 99882960, 99882960, 0, 97612893,
            97612893, 0, 95443717, 95443717, 0, 93368854, 93368854, 0, 91382282, 91382282,
            0, 89478485, 89478485, 0, 87652393, 87652393, 0, 85899345, 85899345, 0,
            84215045, 84215045, 0, 82595524, 82595524, 0, 81037118, 81037118, 0, 79536431,
            79536431, 0, 78090314, 78090314, 0, 76695844, 76695844, 0, 75350303, 75350303,
            0, 74051160, 74051160, 0, 72796055, 72796055, 0, 71582788, 71582788, 0,
            70409299, 70409299, 0, 69273666, 69273666, 0, 68174084, 68174084, 0, int.MinValue,
            0, 5};

        private readonly long _maxEntryValue;
        private readonly byte _valuesPerLong;
        private readonly int _magicIndex;
        private readonly long _divideMul;
        private readonly long _divideAdd;
        private readonly int _divideShift;
        private readonly long[] _storage;

        public int BitsPerEntry { get; }

        /// <summary>
        /// Gets the compacted array length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the actual storage container.
        /// </summary>
        public long[] Storage => _storage;

        /// <summary>
        /// Gets or sets a value at the given index.
        /// </summary>
        /// <param name="index">Index to access the value.</param>
        /// <returns>The value at the given index.</returns>
        public int this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        /// <summary>
        /// Creates a new <see cref="CompactedLongArray"/> instance.
        /// </summary>
        /// <param name="bitsPerEntryIn">Number of bits per single entry.</param>
        /// <param name="arraySizeIn">Array size.</param>
        public CompactedLongArray(int bitsPerEntryIn, int arraySizeIn)
        {
            Length = arraySizeIn;
            BitsPerEntry = bitsPerEntryIn;
            _maxEntryValue = (1L << bitsPerEntryIn) - 1L;
            _valuesPerLong = (byte)(64 / bitsPerEntryIn);
            _magicIndex = 3 * (_valuesPerLong - 1);
            _divideMul = CompactedMagicNumbers[_magicIndex] & 0xffffffffL;
            _divideAdd = CompactedMagicNumbers[_magicIndex + 1] & 0xffffffffL;
            _divideShift = CompactedMagicNumbers[_magicIndex + 2];

            int size = (Length + _valuesPerLong - 1) / _valuesPerLong;

            _storage = new long[size];
        }

        /// <summary>
        /// Gets the value at the given index.
        /// </summary>
        /// <param name="index">Index to access the value.</param>
        /// <returns>Value at the given index.</returns>
        public int Get(int index)
        {
            int cellIndex = GetCellIndex(index);
            int bitIndex = GetBitIndex(index, cellIndex);
            long storedValue = _storage[cellIndex];

            return (int)(storedValue >> bitIndex & _maxEntryValue);
        }

        /// <summary>
        /// Sets a value at the given index.
        /// </summary>
        /// <param name="index">Index to set the value.</param>
        /// <param name="value">Value to be set.</param>
        public void Set(int index, int value)
        {
            int cellIndex = GetCellIndex(index);
            int bitIndex = GetBitIndex(index, cellIndex);

            _storage[cellIndex] = _storage[cellIndex] & ~(_maxEntryValue << bitIndex) | (value & _maxEntryValue) << bitIndex;
        }

        private int GetCellIndex(int index) => (int)(index * _divideMul + _divideAdd >> 32 >> _divideShift);

        private int GetBitIndex(int index, int cellIndex) => (index - cellIndex * _valuesPerLong) * BitsPerEntry;
    }
}