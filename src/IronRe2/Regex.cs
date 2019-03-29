using System;
using System.Runtime.InteropServices;
using System.Text;

namespace IronRe2
{
    /// <summary>
    /// The main regular expression class
    /// </summary>
    public class Regex : IDisposable
    {
        // Raw handle to the unmanaged regex object
        private readonly IntPtr _rawHandle;

        public Regex(string pattern)
        {
            var patternBytes = Encoding.UTF8.GetBytes(pattern);
            _rawHandle = Re2Ffi.cre2_new(patternBytes, patternBytes.Length, IntPtr.Zero);
            if (Re2Ffi.cre2_error_code(_rawHandle) != Re2Ffi.cre2_error_code_t.CRE2_NO_ERROR)
            {
                var errorString = Re2Ffi.cre2_error_string(_rawHandle);
                var error = Marshal.PtrToStringAnsi(errorString);
                throw new RegexCompilationException(error);
            }
        }

        ~Regex()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  Get the pattern for this regex instance
        /// </summary>
        public string Pattern
        {
            get
            {
                var pattern = Re2Ffi.cre2_pattern(_rawHandle);
                return Marshal.PtrToStringAnsi(pattern);
            }
        }

        /// <summary>
        ///  Get the size of the compiled automata
        /// </summary>
        public int ProgramSize => Re2Ffi.cre2_program_size(_rawHandle);

        /// <summary>
        ///  Get the number of capture groups in this pattern
        /// </summary>
        public int CaptureGroupCount =>
            Re2Ffi.cre2_num_capturing_groups(_rawHandle);
        
        /// <summary>
        ///  Find a capture group index by name
        /// </summary>
        /// <param name="name">The named capture to search for</param>
        /// <returns>The capture group index, or -1 if no named group exists</returns>
        public int FindNamedCapture(string name) =>
            Re2Ffi.cre2_find_named_capturing_groups(_rawHandle, name);

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            Re2Ffi.cre2_delete(_rawHandle);
        }
    }
}
