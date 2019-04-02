using System.Collections;
using System.Collections.Generic;

namespace IronRe2
{
    public class Captures : Match, IEnumerable<Match>
    {
        private Re2Ffi.cre2_range_t[] _ranges;

        internal Captures()
            : base()
        {
        }

        internal Captures(Re2Ffi.cre2_range_t[] ranges)
            : base(ranges[0])
        {
            _ranges = ranges;
        }

        /// <summary>
        /// Access the match at the given capture group index
        /// </summary>
        public Match this[int index] => new Match(_ranges[index]);

        /// <summary>
        /// Returns the number of groups in this set of captures.
        /// </summary>
        public int Length => _ranges.Length;

        public IEnumerator<Match> GetEnumerator()
        {
            var len = Length;
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
