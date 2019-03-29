using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace IronRe2
{
    public class RegexCompilationException : Exception
    {
        public RegexCompilationException(string message)
            : base(message)
        {
        }
    }
}
