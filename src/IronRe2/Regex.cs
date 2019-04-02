﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace IronRe2
{
    /// <summary>
    /// The main regular expression class
    /// </summary>
    public class Regex : IDisposable
    {
        // Raw handle to the unmanaged regex object
        private IntPtr _rawHandle;

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
                _rawHandle,
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
            return (ranges.Length == 0) ? IronRe2.Captures.Empty : new Captures(ranges);
        }

        private Re2Ffi.cre2_range_t[] RawMatch(string haystack, int numCaptures)
        {
            var hayBytes = Encoding.UTF8.GetBytes(haystack);
            var captures = new Re2Ffi.cre2_string_t[numCaptures];
            var pin = GCHandle.Alloc(hayBytes);
            try
            {
                // TODO: Support anchor as a parameter
                var matchResult = Re2Ffi.cre2_match(
                    _rawHandle,
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
                Re2Ffi.cre2_strings_to_ranges(hayBytes, ranges, captures, captures.Length);
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

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            var handle = Interlocked.Exchange(ref _rawHandle, IntPtr.Zero);
            if (handle != IntPtr.Zero)
            {
                Re2Ffi.cre2_delete(_rawHandle);
            }
            GC.SuppressFinalize(this);
        }
    }
}