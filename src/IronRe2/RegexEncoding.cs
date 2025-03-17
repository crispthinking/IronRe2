namespace IronRe2;

/// <summary>
///     Supported encodings for regex patterns
/// </summary>
public enum RegexEncoding
{
    /// <summary>
    ///     Regex bytes should be treated as using the
    ///     <see cref="System.Text.Encoding.UTF8" /> encoding.
    /// </summary>
    Utf8,

    /// <summary>
    ///     Regex bytes should be treated as using the
    ///     <c>ISO-8859-1</c> encoding.
    /// </summary>
    Latin1
}
