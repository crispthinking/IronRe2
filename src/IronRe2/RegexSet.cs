using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace IronRe2
{
    /// <summary>
    /// Regex Sets
    /// <para>
    ///   A regex set offers an easy way to find which of a group of expressions
    ///   matches a given search text in a single pass.
    /// </para>
    /// </summary>
    public class RegexSet : IDisposable
    {
        private IntPtr _rawHandle;

        /// <summary>
        /// Create a new regex set containing the given patterns
        /// </summary>
        /// <param name="patterns">The patterns to include in the set</param>
        public RegexSet(IEnumerable<string> patterns)
        {
            var patternsAsBytes = patterns
                .Select(Encoding.UTF8.GetBytes)
                .ToArray();
            _rawHandle = CompileSet(patternsAsBytes);
            Count = patternsAsBytes.Length;
        }

        /// <summary>
        ///  Compile a given set of patterns to a raw regex set handle
        /// </summary>
        /// <param name="patternsAsBytes">The collection of patterns</param>
        /// <returns>The raw set handle or throws an exception</returns>
        private IntPtr CompileSet(IReadOnlyCollection<byte[]> patternsAsBytes)
        {
            // FIXME: Stop leaking this options and pass in proper options here
            var fudge = Re2Ffi.cre2_opt_new();
            // TODO: we could maybe have a `RegexSetBuilder` to represent this
            // stage of regex set compilation.
            var handle = Re2Ffi.cre2_set_new(
                fudge, Re2Ffi.cre2_anchor_t.CRE2_UNANCHORED);

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
                    var error = Encoding.UTF8.GetString(errBuff);
                    Re2Ffi.cre2_set_delete(handle);
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

        
        ~RegexSet()
        {
            Dispose(false);
        }
        
        public int Count {get;}

        public SetMatch Match(string haystack)
        {
            var hayBytes = Encoding.UTF8.GetBytes(haystack);
            var matchIndices = new int[Count];
            var matchCount = Re2Ffi.cre2_set_match(
                _rawHandle,
                hayBytes, new UIntPtr((uint)hayBytes.Length),
                matchIndices, new UIntPtr((uint)matchIndices.Length));
            Array.Resize(ref matchIndices, (int)matchCount);
            return new SetMatch(matchCount, matchIndices);
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
