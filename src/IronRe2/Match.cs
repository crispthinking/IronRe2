namespace IronRe2
{
    /// <summary>
    /// Match Object
    /// <para>
    ///  Represents the extent of a pattern's match in a given search string.
    /// </para>
    /// </summary>
    public class Match
    {
        internal Match()
        {
            Matched = false;
        }

        internal Match(Re2Ffi.cre2_range_t range)
        {
            // If the indexes on the range are invalid then we didn't match
            if (range.start < 0 || range.past < 0)
            {
                Matched = false;
                Start = -1;
                End = -1;
            }
            else
            {
                Matched = true;
                Start = range.start;
                End = range.past;
            }
        }

        /// <summary>
        /// True if the pattern matched the string.
        /// </summary>
        public bool Matched { get; }

        /// <summary>
        ///  If the pattern matched the start index of the match, in bytes.
        /// </summary>
        public long Start { get; }

        /// <summary>
        ///  If the pattern matched the end index of the match, in bytes.
        /// </summary>
        public long End { get; }

        /// <summary>
        ///  A singleton match which represents all empty matches
        /// </summary>
        public static readonly Match Empty = new Match();
    }
}
