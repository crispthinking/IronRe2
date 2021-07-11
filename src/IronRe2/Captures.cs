using System;
using System.Collections;
using System.Collections.Generic;

namespace IronRe2
{
    /// <summary>
    /// Regex match with capture information.
    /// </summary>
    public class Captures : Match, IReadOnlyList<Match>
    {
        private ByteRange[]? _ranges;

        internal Captures()
            : base()
        {
        }

        internal Captures(ReadOnlyMemory<byte> haystack, ByteRange[] ranges)
            : base(haystack, ranges[0])
        {
            _ranges = ranges;
        }

        /// <summary>
        /// Access the match at the given capture group index
        /// </summary>
        public Match this[int index] =>
            (index >= 0 && index < Count) ?
                new Match(_haystack, _ranges![index]) :
                throw new IndexOutOfRangeException($"No capture group at index {index}");

        /// <summary>
        /// Returns the number of groups in this set of captures.
        /// </summary>
        public int Count => _ranges?.Length ?? 0;

        public IEnumerator<Match> GetEnumerator()
        {
            var len = Count;
            for (int i = 0; i < len; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        ///  A singleton match which represents all empty captures
        /// </summary>
        public new static readonly Captures Empty = new Captures();
    }
}
