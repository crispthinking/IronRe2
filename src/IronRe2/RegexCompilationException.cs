using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace IronRe2
{
    /// <summary>Represents an error in parsing or compiling a pattern.</summary>
    public class RegexCompilationException : Exception
    {
        /// <summary>
        /// Get the offending portion of the regular expression being compiled
        /// </summary>
        public string? OffendingPortion { get; }

        /// <summary>
        ///   Create an instance of the regex compilation exception with
        ///   a known offending portion.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="badBit">The offending portion of the expression.</param>
        public RegexCompilationException(string message, string badBit)
            : base(message)
        {
            OffendingPortion = badBit;
        }

        /// <summary>
        ///   Create an instance of the regex compilation exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public RegexCompilationException(string message) : base(message)
        {
        }
    }
}
