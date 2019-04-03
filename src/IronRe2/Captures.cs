using System.Collections;
using System.Collections.Generic;

namespace IronRe2
{
    public class Captures : Match, IReadOnlyList<Match>
    {
        private Re2Ffi.cre2_range_t[] _ranges;

        internal Captures()
            : base()
        {
        }

        internal Captures(byte[] haystack, Re2Ffi.cre2_range_t[] ranges)
            : base(haystack, ranges[0])
        {
            _ranges = ranges;
        }

        /// <summary>
        /// Access the match at the given capture group index
        /// </summary>
        public Match this[int index] => new Match(_haystack, _ranges[index]);

        /// <summary>
        /// Returns the number of groups in this set of captures.
        /// </summary>
        public int Count => _ranges.Length;

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
