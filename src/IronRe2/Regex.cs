using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace IronRe2;

/// <summary>
///     The main regular expression class
/// </summary>
public class Regex : UnmanagedResource<RegexHandle>
{
    /// <summary>
    ///     Create a regular expression from a given pattern
    /// </summary>
    public Regex(string pattern)
        : this(Encoding.UTF8.GetBytes(pattern))
    {
    }

    /// <summary>
    ///     Create a regular expression from a given pattern, encoded as UTF8
    /// </summary>
    public Regex(ReadOnlySpan<byte> pattern)
        : base(Compile(pattern, null))
    {
    }

    /// <summary>
    ///     Create a regular expression from a given pattern
    /// </summary>
    /// <param name="pattern">The pattern to match</param>
    /// <param name="options">The compilation options to use</param>
    public Regex(string pattern, Options options)
        : this(Encoding.UTF8.GetBytes(pattern), options)
    {
    }

    /// <summary>
    ///     Create a regular expression from a given pattern, encoded as UTF8
    /// </summary>
    /// <param name="pattern">The pattern to match, as bytes</param>
    /// <param name="options">The compilation options to use</param>
    public Regex(ReadOnlySpan<byte> pattern, Options options)
        : base(Compile(pattern, options))
    {
    }

    /// <summary>
    ///     Get the pattern for this regex instance
    /// </summary>
    public string Pattern
    {
        get
        {
            var pattern = Re2Ffi.cre2_pattern(RawHandle);
            return Marshal.PtrToStringAnsi(pattern) ?? string.Empty;
        }
    }

    /// <summary>
    ///     Get the size of the compiled automata
    /// </summary>
    public int ProgramSize => Re2Ffi.cre2_program_size(RawHandle);

    /// <summary>
    ///     Get the number of capture groups in this pattern
    /// </summary>
    public int CaptureGroupCount =>
        Re2Ffi.cre2_num_capturing_groups(RawHandle);

    /// <summary>
    ///     Compile the regular expression
    /// </summary>
    /// <param name="patternBytes">
    ///     The regex pattern, as a UTF-8 byte array
    /// </param>
    /// <param name="opts">
    ///     The regex compilation options, or <c>null</c> to use the default
    /// </param>
    /// <returns>
    ///     The raw handle to the Regex, or throws on compilation failure
    /// </returns>
    private static RegexHandle Compile(ReadOnlySpan<byte> patternBytes, Options? opts)
    {
        RegexHandle handle;
        if (opts == null)
        {
            using OptionsHandle tmpOpts = new();
            handle = Re2Ffi.cre2_new(
                in MemoryMarshal.GetReference(patternBytes), patternBytes.Length,
                tmpOpts);
        }
        else
        {
            handle = Re2Ffi.cre2_new(
                in MemoryMarshal.GetReference(patternBytes), patternBytes.Length,
                opts.RawHandle);
        }

        // Check to see if there was an error compiling this expression
        var errorCode = Re2Ffi.cre2_error_code(handle);
        if (errorCode != Re2Ffi.cre2_error_code_t.CRE2_NO_ERROR)
        {
            var errorString = Re2Ffi.cre2_error_string(handle);
            var error = Marshal.PtrToStringAnsi(errorString);
            Re2Ffi.cre2_string_t errorArg = new();
            Re2Ffi.cre2_error_arg(handle, ref errorArg);
            var offendingPortion = Marshal.PtrToStringAnsi(
                errorArg.data, errorArg.length);
            // Clean up the regex
            handle.Dispose();
            if (error == null)
            {
                throw new RegexCompilationException(
                    "Unknown error compiling regex", offendingPortion);
            }

            throw new RegexCompilationException(error, offendingPortion);
        }

        return handle;
    }

    /// <summary>
    ///     Find a capture group index by name
    /// </summary>
    /// <param name="name">The named capture to search for</param>
    /// <returns>The capture group index, or -1 if no named group exists</returns>
    public int FindNamedCapture(string name)
    {
        return Re2Ffi.cre2_find_named_capturing_groups(RawHandle, name);
    }

    /// <summary>
    ///     Checks if the pattern matches somewhere in the given
    ///     <paramref name="haystack" />.
    /// </summary>
    /// <param name="haystack">The text to find the pattern in</param>
    /// <returns>True if the pattern matches, false otherwise.</returns>
    public bool IsMatch(string haystack)
    {
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return IsMatch(hayBytes);
    }

    /// <summary>
    ///     Checks if the pattern matches somewhere in the given
    ///     <paramref name="haystack" />.
    /// </summary>
    /// <param name="haystack">The text to find the pattern in</param>
    /// <returns>True if the pattern matches, false otherwise.</returns>
    public unsafe bool IsMatch(ReadOnlySpan<byte> haystack)
    {
        Re2Ffi.cre2_string_t[] captures = [];

        fixed (byte* hayBytesPtr = haystack)
        fixed (Re2Ffi.cre2_string_t* capturesPtr = captures)
        {
            // TODO: Support anchor as a parameter
            var matchResult = Re2Ffi.cre2_match(
                RawHandle,
                hayBytesPtr, haystack.Length,
                0, haystack.Length,
                Re2Ffi.cre2_anchor_t.CRE2_UNANCHORED,
                capturesPtr, 0);
            return matchResult == 1;
        }
    }

    /// <summary>
    ///     Find the pattern and return the extent of the match
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>The match data</returns>
    public Match Find(string haystack)
    {
        return Find(haystack, 0);
    }

    /// <summary>
    ///     Find the pattern starting at the given offset and return the extent
    ///     of the match
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <param name="offset">The offset to start the search at</param>
    /// <returns>The match data for the match</returns>
    public Match Find(string haystack, int offset)
    {
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return Find(hayBytes, offset);
    }

    /// <summary>
    ///     Find the pattern and return the extent of the match
    /// </summary>
    /// <param name="hayBytes">The string to search for the pattern</param>
    /// <returns>The match data</returns>
    public Match Find(ReadOnlyMemory<byte> hayBytes)
    {
        return Find(hayBytes, 0);
    }

    /// <summary>
    ///     Find the pattern starting at the given offset and return the extent
    ///     of the match
    /// </summary>
    /// <param name="hayBytes">The string to search for the pattern</param>
    /// <param name="offset">The offset to start the search at</param>
    /// <returns>The match data for the match</returns>
    public Match Find(ReadOnlyMemory<byte> hayBytes, int offset)
    {
        var ranges = RawMatch(hayBytes.Span, offset, 1);
        return ranges.Length != 1 ? Match.Empty : new Match(hayBytes, ranges[0]);
    }

    /// <summary>
    ///     Find all Non-Overlapping Occurrences of the Pattern
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>An enumerable of the matches</returns>
    public IEnumerable<Match> FindAll(string haystack)
    {
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return FindAll(hayBytes);
    }

    /// <summary>
    ///     Find all Non-Overlapping Occurrences of the Pattern
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>An enumerable of the matches</returns>
    public IEnumerable<Match> FindAll(ReadOnlyMemory<byte> haystack)
    {
        var offset = 0;
        while (true)
        {
            var match = Find(haystack, offset);
            if (match.Matched)
            {
                offset = match.Start == match.End ? (int)match.End + 1 : (int)match.End;
                yield return match;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    ///     Get an Iterator over the Named Captures in the Pattern
    /// </summary>
    /// <returns>An enumerable of the named capture groups</returns>
    public IEnumerable<NamedCaptureGroup> NamedCaptures()
    {
        return new NamedCaptureEnumerable(this);
    }

    /// <summary>
    ///     Find with Captures
    ///     <para>
    ///         This is the most expensive of the match options but provides the
    ///         richest information about the match. The returned
    ///         <see cref="IronRe2.Captures" /> object contains the match position
    ///         of each of the regex's capturing groups.
    ///     </para>
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>The captures data</returns>
    public Captures Captures(string haystack)
    {
        return Captures(haystack, 0);
    }

    /// <summary>
    ///     Find with Captures, starting from a given offset
    ///     <para>
    ///         This is the most expensive of the match options but provides the
    ///         richest information about the match. The returned
    ///         <see cref="IronRe2.Captures" /> object contains the match position
    ///         of each of the regex's capturing groups.
    ///     </para>
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <param name="offset">The offest to start searching from</param>
    /// <returns>The captures data</returns>
    public Captures Captures(string haystack, int offset)
    {
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return Captures(hayBytes, offset);
    }

    /// <summary>
    ///     Find with Captures
    ///     <para>
    ///         This is the most expensive of the match options but provides the
    ///         richest information about the match. The returned
    ///         <see cref="IronRe2.Captures" /> object contains the match position
    ///         of each of the regex's capturing groups.
    ///     </para>
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>The captures data</returns>
    public Captures Captures(ReadOnlyMemory<byte> haystack)
    {
        return Captures(haystack, 0);
    }

    /// <summary>
    ///     Find with Captures, starting from a given offset
    ///     <para>
    ///         This is the most expensive of the match options but provides the
    ///         richest information about the match. The returned
    ///         <see cref="IronRe2.Captures" /> object contains the match position
    ///         of each of the regex's capturing groups.
    ///     </para>
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <param name="offset">The offest to start searching from</param>
    /// <returns>The captures data</returns>
    public Captures Captures(ReadOnlyMemory<byte> haystack, int offset)
    {
        var ranges = RawMatch(haystack.Span, offset, CaptureGroupCount + 1);
        return ranges.Length == 0 ? IronRe2.Captures.Empty : new Captures(haystack, ranges);
    }

    /// <summary>
    ///     Find all the non-overlapping sets of captures in the given text.
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>An iterator of the captures at each match location</returns>
    public IEnumerable<Captures> CaptureAll(string haystack)
    {
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return CaptureAll(hayBytes);
    }

    /// <summary>
    ///     Find all the non-overlapping sets of captures in the given text.
    /// </summary>
    /// <param name="haystack">The string to search for the pattern</param>
    /// <returns>An iterator of the captures at each match location</returns>
    public IEnumerable<Captures> CaptureAll(ReadOnlyMemory<byte> haystack)
    {
        var offset = 0;
        while (true)
        {
            var caps = Captures(haystack, offset);
            if (caps.Matched)
            {
                offset = caps.Start == caps.End ? (int)caps.End + 1 : (int)caps.End;
                yield return caps;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    ///     Managed wrapper around the raw match API.
    ///     <para>
    ///         From the RE2 docs for the underlying function:
    ///         Don't ask for more match information than you will use:
    ///         runs much faster with nsubmatch == 1 than nsubmatch > 1, and
    ///         runs even faster if nsubmatch == 0.
    ///         Doesn't make sense to use nsubmatch > 1 + NumberOfCapturingGroups(),
    ///         but will be handled correctly.
    ///     </para>
    /// </summary>
    /// <param name="hayBytes">The string to match the pattern against</param>
    /// <param name="startByteIndex">The byte offset to start at</param>
    /// <param name="numCaptures">The number of match groups to return</param>
    /// <returns>An array of byte ranges for the captures</returns>
    private unsafe ByteRange[] RawMatch(
        ReadOnlySpan<byte> hayBytes, int startByteIndex, int numCaptures)
    {
        var captures = new Re2Ffi.cre2_string_t[numCaptures];
        fixed (byte* pinnedHayBytes = hayBytes)
        fixed (Re2Ffi.cre2_string_t* capturesPtr = captures)
        {
            var matchResult = Re2Ffi.cre2_match(
                RawHandle,
                pinnedHayBytes, hayBytes.Length,
                startByteIndex, hayBytes.Length,
                Re2Ffi.cre2_anchor_t.CRE2_UNANCHORED,
                capturesPtr, captures.Length);
            if (matchResult != 1)
            {
                return [];
            }

            // Convert the captured strings to array indices while we still
            // have the haystack pinned. We can't have the haystack move
            // between the `_match` and the conversion to byte ranges
            // otherwise the pointer arithmetic we do will be invalidated.
            return StringsToRanges(captures, new IntPtr(pinnedHayBytes));
        }
    }

    /// <summary>
    ///     cre2 Strings to Byte Ranges
    /// </summary>
    /// <param name="captures">The captures to convert to byte ranges</param>
    /// <param name="hayBasePtr">The base pointer of the search text.</param>
    /// <returns>An array of offsets into the haystack corresponding to the input strings</returns>
    private static ByteRange[] StringsToRanges(
        Re2Ffi.cre2_string_t[] captures, IntPtr hayBasePtr)
    {
        var hayBase = hayBasePtr.ToInt64();
        var ranges = new ByteRange[captures.Length];
        for (var i = 0; i < ranges.Length; i++)
        {
            var c = captures[i];
            var start = c.data.ToInt64() - hayBase;
            ranges[i] = new ByteRange(start, start + c.length);
        }

        return ranges;
    }

    /// <summary>
    ///     Easy IsMatch
    ///     <para>Checks if the given pattern exists in the given haystack</para>
    /// </summary>
    /// <param name="pattern">The regular expression to search for</param>
    /// <param name="haystack">The text to find the pattern in</param>
    /// <returns>True if the pattern matches in the given text</returns>
    public static bool IsMatch(string pattern, string haystack)
    {
        var patternBytes = Encoding.UTF8.GetBytes(pattern);
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return IsMatch(patternBytes, hayBytes);
    }

    /// <summary>
    ///     Easy IsMatch
    ///     <para>Checks if the given pattern exists in the given haystack</para>
    /// </summary>
    /// <param name="pattern">The regular expression to search for</param>
    /// <param name="haystack">The text to find the pattern in</param>
    /// <returns>True if the pattern matches in the given text</returns>
    public static bool IsMatch(
        ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> haystack)
    {
        Re2Ffi.cre2_string_t[] captures = [];
        var matchResult = Re2Ffi.cre2_easy_match(
            in MemoryMarshal.GetReference(pattern), pattern.Length,
            in MemoryMarshal.GetReference(haystack), haystack.Length,
            captures, 0);
        return matchResult == 1;
    }

    /// <summary>
    ///     Easy Find
    ///     <para>
    ///         Finds the extent of the match of <paramref name="pattern" /> in
    ///         the given <paramref name="haystack" />. To check if a given pattern
    ///         matches without needing to find the rage use <see cref="IsMatch(String)" />.
    ///     </para>
    /// </summary>
    /// <param name="pattern">The regular expression to search for</param>
    /// <param name="haystack">The text to find the pattern in</param>
    /// <returns>A match object summarising the match result</returns>
    public static Match Find(string pattern, string haystack)
    {
        var patternBytes = Encoding.UTF8.GetBytes(pattern);
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return Find(patternBytes, hayBytes);
    }

    /// <summary>
    ///     Easy Find
    ///     <para>
    ///         Finds the extent of the match of <paramref name="pattern" /> in
    ///         the given <paramref name="haystack" />. To check if a given pattern
    ///         matches without needing to find the rage use <see cref="IsMatch(ReadOnlySpan&lt;byte&gt;)" />.
    ///     </para>
    /// </summary>
    /// <param name="pattern">The regular expression to search for</param>
    /// <param name="haystack">The text to find the pattern in</param>
    /// <returns>A match object summarising the match result</returns>
    public static unsafe Match Find(
        ReadOnlySpan<byte> pattern, ReadOnlyMemory<byte> haystack)
    {
        // Use an explicit one-element array for captures.
        var captures = new Re2Ffi.cre2_string_t[1];
        using var pin = haystack.Pin();
        var matchResult = Re2Ffi.cre2_easy_match(
            in MemoryMarshal.GetReference(pattern), pattern.Length,
            haystack.Span[0], haystack.Length,
            captures, 1);
        if (matchResult != 1)
        {
            return Match.Empty;
        }

        // Convert the captured strings to array indices while we still
        // have the haystack pinned. We can't have the haystack move
        // between the `_match` and the conversion to byte ranges
        // otherwise the pointer arithmetic we do will be invalidated.
        var ranges = StringsToRanges(captures, new IntPtr(pin.Pointer));
        return new Match(haystack, ranges[0]);
    }
}
