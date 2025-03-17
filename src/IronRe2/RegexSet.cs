using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IronRe2;

/// <summary>
///     Regex Sets
///     <para>
///         A regex set offers an easy way to find which of a group of expressions
///         matches a given search text in a single pass.
///     </para>
/// </summary>
public class RegexSet : UnmanagedResource<RegexSetHandle>
{
    /// <summary>
    ///     Create a new regex set containing the given patterns
    /// </summary>
    /// <param name="patterns">The patterns to include in the set</param>
    public RegexSet(IEnumerable<string> patterns)
        : this(patterns.Select(Encoding.UTF8.GetBytes).ToArray())
    {
    }

    /// <summary>
    ///     Create a new regex set containing the given patterns
    /// </summary>
    /// <param name="patterns">
    ///     The patterns to include in the set
    /// </param>
    /// <param name="opts">
    ///     The regular expression options to use when compiling
    /// </param>
    public RegexSet(IEnumerable<string> patterns, Options opts)
        : this(patterns.Select(Encoding.UTF8.GetBytes).ToArray(), opts)
    {
    }

    /// <summary>
    ///     Create a new regex set containing the given byte patterns
    /// </summary>
    /// <param name="patterns">The patterns to include in the set, as UTF8</param>
    public RegexSet(IReadOnlyCollection<byte[]> patterns)
        : base(CompileSet(patterns))
    {
        Count = patterns.Count;
    }

    /// <summary>
    ///     Create a new regex set containing the given byte patterns
    /// </summary>
    /// <param name="patterns">
    ///     The patterns to include in the set, as UTF8
    /// </param>
    /// <param name="opts">
    ///     The regular expression options to use when compiling
    /// </param>
    public RegexSet(IReadOnlyCollection<byte[]> patterns, Options opts)
        : base(CompileSetWithOptions(patterns, opts))
    {
        Count = patterns.Count;
    }

    /// <summary>
    ///     Returns the number of patterns in this set
    /// </summary>
    public int Count { get; }

    /// <summary>
    ///     Compile a given set of patterns to a raw regex set handle
    /// </summary>
    /// <param name="patternsAsBytes">The collection of patterns</param>
    /// <returns>The raw set handle or throws an exception</returns>
    private static RegexSetHandle CompileSet(IReadOnlyCollection<byte[]> patternsAsBytes)
    {
        using Options? opts = new();
        return CompileSetWithOptions(patternsAsBytes, opts);
    }

    /// <summary>
    ///     Compile a given set of patterns to a raw regex set handle
    /// </summary>
    /// <param name="patternsAsBytes">The collection of patterns</param>
    /// <param name="options">
    ///     The regular expression options to use when compiling
    /// </param>
    /// <returns>The raw set handle or throws an exception</returns>
    private static RegexSetHandle CompileSetWithOptions(
        IReadOnlyCollection<byte[]> patternsAsBytes,
        Options options)
    {
        // TODO: we could maybe have a `RegexSetBuilder` to represent this
        // stage of regex set compilation.
        var handle = Re2Ffi.cre2_set_new(
            options.RawHandle, Re2Ffi.cre2_anchor_t.CRE2_UNANCHORED);

        var errBuff = new byte[100];
        foreach (var pattern in patternsAsBytes)
        {
            var r = Re2Ffi.cre2_set_add(
                handle,
                pattern, new UIntPtr((uint)pattern.Length),
                errBuff, new UIntPtr((uint)errBuff.Length));
            if (r < 0)
            {
                // If the regex failed to add then throw
                var error = Encoding.UTF8.GetString(errBuff).TrimEnd('\0');
                handle.Dispose();
                throw new RegexCompilationException(
                    error, Encoding.UTF8.GetString(pattern));
            }
        }

        if (Re2Ffi.cre2_set_compile(handle) != 1)
        {
            throw new RegexCompilationException("Error compiling regex set");
        }

        return handle;
    }

    /// <summary>
    ///     Match the patterns against he given search text and return
    ///     information about the matching patterns.
    /// </summary>
    /// <param name="haystack">The text to search</param>
    /// <returns>An object representing the state of the matches</returns>
    public SetMatch Match(string haystack)
    {
        var hayBytes = Encoding.UTF8.GetBytes(haystack);
        return Match(hayBytes);
    }

    /// <summary>
    ///     Match the patterns against he given search text and return
    ///     information about the matching patterns.
    /// </summary>
    /// <param name="haystack">The text to search</param>
    /// <returns>An object representing the state of the matches</returns>
    public SetMatch Match(ReadOnlySpan<byte> haystack)
    {
        var matchIndices = new int[Count];
        var matchCount = Re2Ffi.cre2_set_match(
            RawHandle,
            in MemoryMarshal.GetReference(haystack), new UIntPtr((uint)haystack.Length),
            matchIndices, new UIntPtr((uint)matchIndices.Length));
        Array.Resize(ref matchIndices, (int)matchCount);
        return new SetMatch(matchCount, matchIndices);
    }
}
