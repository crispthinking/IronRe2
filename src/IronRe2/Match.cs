using System;
using System.Text;

namespace IronRe2;

/// <summary>
///     Match Object
///     <para>
///         Represents the extent of a pattern's match in a given search string.
///     </para>
/// </summary>
public class Match
{
    /// <summary>
    ///     A singleton match which represents all empty matches
    /// </summary>
    public static readonly Match Empty = new();

    /// <summary>The haystack this match was performed against</summary>
    protected readonly ReadOnlyMemory<byte> _haystack;

    internal Match()
    {
        Matched = false;
    }

    internal Match(ReadOnlyMemory<byte> haystack, ByteRange range)
    {
        _haystack = haystack;
        // If the indexes on the range are invalid then we didn't match
        if (range.Start < 0 || range.Past < 0)
        {
            Matched = false;
            Start = -1;
            End = -1;
        }
        else
        {
            Matched = true;
            Start = range.Start;
            End = range.Past;
        }
    }

    /// <summary>
    ///     True if the pattern matched the string.
    /// </summary>
    public bool Matched { get; }

    /// <summary>
    ///     If the pattern matched the start index of the match, in bytes.
    /// </summary>
    public long Start { get; }

    /// <summary>
    ///     If the pattern matched the end index of the match, in bytes.
    /// </summary>
    public long End { get; }

    /// <summary>
    ///     Get the text for this match
    /// </summary>
    public unsafe string ExtractedText
    {
        get
        {
            if (!Matched || Start == End)
            {
                return string.Empty;
            }

            var haySlice = _haystack.Span.Slice((int)Start, (int)(End - Start));
            fixed (byte* haySlicePtr = haySlice)
            {
                return Encoding.UTF8.GetString(haySlicePtr, haySlice.Length);
            }
        }
    }
}
