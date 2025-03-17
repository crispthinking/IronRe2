using System;

namespace IronRe2;

/// <summary>
///     Managed alternative to the `cre2_range_t` structure.
///     <para>
///         We can't use the `cre2_strings_to_ranges` method directly as the size
///         of the `cre2_range_t` structure varies depending on if the platform is
///         LP64 or LLP64. To solve this we do the pointer arithmetic in managed
///         code and store the result in this structure instead.
///     </para>
/// </summary>
internal readonly struct ByteRange : IEquatable<ByteRange>
{
    /// <summary>
    ///     Gets the starting index of the range.
    /// </summary>
    internal long Start { get; }

    /// <summary>
    ///     Gets the index one past the last element in the range.
    /// </summary>
    internal long Past { get; }

    /// <summary>
    ///     Gets the length of the range.
    /// </summary>
    internal long Length => Past - Start;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ByteRange" /> struct.
    /// </summary>
    /// <param name="start">The starting index.</param>
    /// <param name="past">The index one past the last element.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="past" /> is less than <paramref name="start" />.</exception>
    public ByteRange(long start, long past)
    {
        if (past < start)
        {
            throw new ArgumentException("The 'past' value must be greater than or equal to the 'start' value.",
                nameof(past));
        }

        Start = start;
        Past = past;
    }

    public static implicit operator ByteRange(Range range)
    {
        // This conversion assumes that the System.Range indices are from the start (not from the end)
        // and that they fit into an int. If using from-end indices, an exception is thrown.
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
        {
            throw new NotSupportedException("System.Range with from-end indices is not supported.");
        }

        return new ByteRange(range.Start.Value, range.End.Value);
    }

    public static implicit operator Range(ByteRange byteRange)
    {
        if (byteRange.Start > int.MaxValue || byteRange.Past > int.MaxValue)
        {
            throw new OverflowException("ByteRange values exceed the range supported by System.Index.");
        }

        return new Range(
            new Index((int)byteRange.Start),
            new Index((int)byteRange.Past));
    }

    // Equality members.
    public override bool Equals(object? obj)
    {
        return obj is ByteRange other && Equals(other);
    }

    public bool Equals(ByteRange other)
    {
        return Start == other.Start && Past == other.Past;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, Past);
    }

    public static bool operator ==(ByteRange left, ByteRange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ByteRange left, ByteRange right)
    {
        return !(left == right);
    }

    /// <summary>
    ///     Returns a string representation of the ByteRange.
    /// </summary>
    public override string ToString()
    {
        return $"[{Start}, {Past})";
    }
}
