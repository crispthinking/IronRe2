using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace IronRe2
{
    public class RegexCompilationException : Exception
    {
        /// <summary>
        /// Get the offending portion of the regular expression being compiled
        /// </summary>
        public string OffendingPortion { get; }

        public RegexCompilationException(string message, string badBit)
            : base(message)
        {
            OffendingPortion = badBit;
        }

        public RegexCompilationException(string message) : base(message)
        {
        }
    }
}
