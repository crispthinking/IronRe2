using System;
using System.Runtime.InteropServices;
using System.Text;

namespace IronRe2
{
    /// <summary>
    /// The main regular expression class
    /// </summary>
    public class Regex : UnmanagedResource
    {
        /// <summary>
        /// Create a regular expression from a given pattern
        /// </summary>
        /// <param name="pattern">The pattern to match</param>
        public Regex(string pattern)
            : this(Encoding.UTF8.GetBytes(pattern))
        {
        }

        /// <summary>
        /// Create a regular expression from a given pattern, encoded as UTF8
        /// </summary>
        /// <param name="pattern">The pattern to match, as bytes</param>
        public Regex(byte[] pattern)
            : base(Compile(pattern, null))
        {
        }

        /// <summary>
        /// Create a regular expression from a given pattern
        /// </summary>
        /// <param name="pattern">The pattern to match</param>
        /// <param name="options">The compilation options to use</param>
        public Regex(string pattern, Options options)
            : this(Encoding.UTF8.GetBytes(pattern), options)
        {
        }

        /// <summary>
        /// Create a regular expression from a given pattern, encoded as UTF8
        /// </summary>
        /// <param name="pattern">The pattern to match, as bytes</param>
        /// <param name="options">The compilation options to use</param>
        public Regex(byte[] pattern, Options options)
            : base(Compile(pattern, options))
        {
        }

        /// <summary>
        /// Called by <see cref="UnmanagedResource" /> whe the resource goes out
        /// of scope.
        /// </summary>
        /// <param name="re">The handle to the regex to free</param>
        protected override void Free(IntPtr re) => Re2Ffi.cre2_delete(re);

        /// <summary>
        /// Compile the regular expression
        /// </summary>
        /// <param name="patternBytes">
        /// The regex pattern, as a UTF-8 byte array
        /// </param>
        /// <param name="opts">
        /// The regex compilation options, or <c>null</c> to use the default
        /// </param>
        /// <returns>
        /// The raw handle to the Regex, or throws on compilation failure
        /// </returns>
        private static IntPtr Compile(byte[] patternBytes, Options opts)
        {
            var handle = Re2Ffi.cre2_new(
                patternBytes, patternBytes.Length,
                opts?.RawHandle ?? IntPtr.Zero);
            
            // Check to see if there was an error compiling this expression
            var errorCode = Re2Ffi.cre2_error_code(handle);
            if (errorCode != Re2Ffi.cre2_error_code_t.CRE2_NO_ERROR)
            {
                var errorString = Re2Ffi.cre2_error_string(handle);
                var error = Marshal.PtrToStringAnsi(errorString);
                var errorArg = new Re2Ffi.cre2_string_t();
                Re2Ffi.cre2_error_arg(handle, ref errorArg);
                var offendingPortion = Marshal.PtrToStringAnsi(
                    errorArg.data, errorArg.length);
                // need to clean up the regex
                Re2Ffi.cre2_delete(handle);
                throw new RegexCompilationException(error, offendingPortion);
            }

            return handle;
        }

        /// <summary>
        ///  Get the pattern for this regex instance
        /// </summary>
        public string Pattern
        {
            get
            {
                var pattern = Re2Ffi.cre2_pattern(RawHandle);
                return Marshal.PtrToStringAnsi(pattern);
            }
        }

        /// <summary>
        ///  Get the size of the compiled automata
        /// </summary>
        public int ProgramSize => Re2Ffi.cre2_program_size(RawHandle);

        /// <summary>
        ///  Get the number of capture groups in this pattern
        /// </summary>
        public int CaptureGroupCount =>
            Re2Ffi.cre2_num_capturing_groups(RawHandle);
        
        /// <summary>
        ///  Find a capture group index by name
        /// </summary>
        /// <param name="name">The named capture to search for</param>
        /// <returns>The capture group index, or -1 if no named group exists</returns>
        public int FindNamedCapture(string name) =>
            Re2Ffi.cre2_find_named_capturing_groups(RawHandle, name);

        /// <summary>
        /// Checks if the pattern matches somewhere in the given
        /// <paramref name="haystack" />.
        /// </summary>
        /// <param name="haystack">The text to find the pattern in</param>
        /// <returns>True if the pattern matches, false otherwise.</returns>
        public bool IsMatch(string haystack)
        {
            var hayBytes = Encoding.UTF8.GetBytes(haystack);
            var captures = Array.Empty<Re2Ffi.cre2_string_t>();
            // TODO: Support anchor as a parameter
            var matchResult = Re2Ffi.cre2_match(
                RawHandle,
                hayBytes, hayBytes.Length,
                0, hayBytes.Length,
                Re2Ffi.cre2_anchor_t.CRE2_UNANCHORED,
                captures, 0);
            return matchResult == 1;
        }

        /// <summary>
        ///  Find the pattern and return the extent of the match
        /// </summary>
        /// <param name="haystack">The string to search for the pattern</param>
        /// <returns>The match data</returns>
        public Match Find(string haystack)
        {
            var ranges = RawMatch(haystack, 1);
            return (ranges.Length != 1) ? Match.Empty : new Match(ranges[0]);
        }

        public Captures Captures(string haystack)
        {
            var ranges = RawMatch(haystack, CaptureGroupCount + 1);
            return (ranges.Length == 0) ?
                IronRe2.Captures.Empty : new Captures(ranges);
        }

        /// <summary>
        /// Managed wrapper around the raw match API.
        /// <para>From the RE2 docs for the underlying function:
        /// Don't ask for more match information than you will use:
        /// runs much faster with nsubmatch == 1 than nsubmatch > 1, and
        /// runs even faster if nsubmatch == 0.
        /// Doesn't make sense to use nsubmatch > 1 + NumberOfCapturingGroups(),
        /// but will be handled correctly.
        /// </para>
        /// </summary>
        /// <param name="haystack">The string to match the pattern against</param>
        /// <param name="numCaptures">The number of match groups to return</param>
        /// <returns></returns>
        private Re2Ffi.cre2_range_t[] RawMatch(string haystack, int numCaptures)
        {
            var hayBytes = Encoding.UTF8.GetBytes(haystack);
            var captures = new Re2Ffi.cre2_string_t[numCaptures];
            var pin = GCHandle.Alloc(hayBytes);
            try
            {
                // TODO: Support anchor as a parameter
                var matchResult = Re2Ffi.cre2_match(
                    RawHandle,
                    hayBytes, hayBytes.Length,
                    0, hayBytes.Length,
                    Re2Ffi.cre2_anchor_t.CRE2_UNANCHORED,
                    captures, captures.Length);
                if (matchResult != 1)
                {
                    return Array.Empty<Re2Ffi.cre2_range_t>();
                }

                // Convert the captured strings to array indices while we still
                // have the haystack pinned. We can't have the haystack move
                // between the `_match` and `_strings_to_ranges` call otherwise
                // the pointer arithmetic it does will be invalidated.
                var ranges = new Re2Ffi.cre2_range_t[captures.Length];
                Re2Ffi.cre2_strings_to_ranges(
                    hayBytes, ranges, captures, captures.Length);
                return ranges;
            }
            finally
            {
                pin.Free();
            }
        }

        /// <summary>
        /// Easy IsMatch
        /// <para>Checks if the given pattern exists in the given haystack</para>
        /// </summary>
        /// <param name="pattern">The regular expression to search for</param>
        /// <param name="haystack">The text to find the pattern in</param>
        /// <returns>True if the pattern matches in the given text</returns>
        public static bool IsMatch(string pattern, string haystack)
        {
            var patternBytes = Encoding.UTF8.GetBytes(pattern);
            var hayBytes = Encoding.UTF8.GetBytes(haystack);
            var captures = Array.Empty<Re2Ffi.cre2_string_t>();
            var matchResult = Re2Ffi.cre2_easy_match(
                patternBytes, patternBytes.Length,
                hayBytes, hayBytes.Length,
                captures, 0);
            return matchResult == 1;
        }

        /// <summary>
        /// Easy Find
        /// <para>Finds the extent of the match of <paramref name="pattern" /> in
        /// the given <paramref name="haystack" />. To check if a given pattern
        /// matches without needing to find the rage use <see cref="IsMatch" />.
        /// </para>
        /// </summary>
        /// <param name="pattern">The regular expression to search for</param>
        /// <param name="haystack">The text to find the pattern in</param>
        /// <returns>A match object summarising the match result</returns>
        public static Match Find(string pattern, string haystack)
        {
            var patternBytes = Encoding.UTF8.GetBytes(pattern);
            var hayBytes = Encoding.UTF8.GetBytes(haystack);
            var captures = new [] {
                new Re2Ffi.cre2_string_t()
            };
            var pin = GCHandle.Alloc(hayBytes);
            try
            {
                var matchResult = Re2Ffi.cre2_easy_match(
                    patternBytes, patternBytes.Length,
                    hayBytes, hayBytes.Length,
                    captures, 1
                );
                if (matchResult != 1)
                {
                    return Match.Empty;
                }

                // Convert the captured strings to array indices while we still
                // have the haystack pinned. We can't have the haystack move
                // between the `_match` and `_strings_to_ranges` call otherwise
                // the pointer arithmetic it does will be invalidated.
                var ranges = new [] {
                    new Re2Ffi.cre2_range_t()
                };
                Re2Ffi.cre2_strings_to_ranges(hayBytes, ranges, captures, 1);
                return new Match(ranges[0]);
            }
            finally
            {
                pin.Free();
            }
        }
    }
}
