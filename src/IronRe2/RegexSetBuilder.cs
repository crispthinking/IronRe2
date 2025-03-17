using System.Collections.Generic;
using System.Text;

namespace IronRe2;

/// <summary>
///     Builder class for creating <see cref="RegexSet" /> instances.
/// </summary>
public sealed class RegexSetBuilder
{
    private readonly Options _options;
    private readonly List<byte[]> _patterns = [];

    /// <summary>
    ///     Create a new <see cref="RegexSetBuilder" /> with the default
    ///     options.
    /// </summary>
    public RegexSetBuilder()
        : this(new Options())
    {
    }

    /// <summary>
    ///     Create a new <see cref="RegexSetBuilder" /> with custom options.
    /// </summary>
    public RegexSetBuilder(Options options)
    {
        _options = options;
    }

    /// <summary>
    ///     Complete the bild and get the resulting set.
    /// </summary>
    public RegexSet Build()
    {
        return new RegexSet(_patterns, _options);
    }

    /// <summary>
    ///     Add a pattern to the builder.
    /// </summary>
    /// <param name="pattern">The pattern to add</param>
    /// <returns>The index of the pattern in the set.</returns>
    public int Add(string pattern)
    {
        return Add(Encoding.UTF8.GetBytes(pattern));
    }

    /// <summary>
    ///     Add a pattern to the builder.
    /// </summary>
    /// <param name="pattern">The pattern to add</param>
    /// <returns>The index of the pattern in the set.</returns>
    public int Add(byte[] pattern)
    {
        var idx = _patterns.Count;
        _patterns.Add(pattern);
        return idx;
    }
}
