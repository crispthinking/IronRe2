using System;

namespace IronRe2;

/// <summary>
///     Represents an error in parsing or compiling a pattern.
/// </summary>
public class RegexCompilationException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RegexCompilationException" /> class.
    /// </summary>
    public RegexCompilationException()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegexCompilationException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public RegexCompilationException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegexCompilationException" /> class with a specified error message
    ///     and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public RegexCompilationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegexCompilationException" /> class with a specified error message
    ///     and the offending portion of the regular expression.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="badBit">The offending portion of the expression.</param>
    public RegexCompilationException(string message, string badBit)
        : base(message)
    {
        OffendingPortion = badBit;
    }

    /// <summary>
    ///     Gets the offending portion of the regular expression being compiled.
    /// </summary>
    public string? OffendingPortion { get; }
}
