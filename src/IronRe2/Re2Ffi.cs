using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

using cre2_regexp_t_ptr = System.IntPtr;
using cre2_options_t_ptr = System.IntPtr;

namespace IronRe2
{
    /// <summary>
    /// This class contains the P/Invoke definitions for the cre2 library's
    /// public interface.
    /// </summary>
    static class Re2Ffi
    {
        /** --------------------------------------------------------------------
         ** Version functions.
         ** ----------------------------------------------------------------- */


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_string();


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_interface_current();


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_interface_revision();


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_interface_age();



        /** --------------------------------------------------------------------
         ** Regular expressions configuration options.
         ** ----------------------------------------------------------------- */

        // TODO: Add FFI defnitions for the `cre2_opt_*` functions

        /** --------------------------------------------------------------------
         ** Precompiled regular expressions.
         ** ----------------------------------------------------------------- */

        public struct cre2_string_t {
            [MarshalAs(UnmanagedType.LPStr)]
            public string data;
            public IntPtr length; 
        };

        /// <summary>
        /// This definition  must be  kept in sync  with the definition  of 
        /// "enum ErrorCode" in the file "re2.h" of the original RE2
        /// distribution.
        /// </summary>
        public enum cre2_error_code_t {
            CRE2_NO_ERROR = 0,
            /// <summary>
            /// unexpected error
            /// </summary>
            CRE2_ERROR_INTERNAL,
            /// <summary>
            /// bad escape sequence
            /// </summary>
            CRE2_ERROR_BAD_ESCAPE,
            /// <summary>
            /// bad character class
            /// </summary>
            CRE2_ERROR_BAD_CHAR_CLASS,
            /// <summary>
            /// bad character class range
            /// </summary>
            CRE2_ERROR_BAD_CHAR_RANGE,
            /// <summary>
            /// missing closing ]
            /// </summary>
            CRE2_ERROR_MISSING_BRACKET,
            /// <summary>
            /// missing closing )
            /// </summary>
            CRE2_ERROR_MISSING_PAREN,
            /// <summary>
            /// trailing \ at end of regexp
            /// </summary>
            CRE2_ERROR_TRAILING_BACKSLASH,
            /// <summary>
            /// repeat argument missing, e.g. "*"
            /// </summary>
            CRE2_ERROR_REPEAT_ARGUMENT,
            /// <summary>
            /// bad repetition argument
            /// </summary>
            CRE2_ERROR_REPEAT_SIZE,
            /// <summary>
            /// bad repetition operator
            /// </summary>
            CRE2_ERROR_REPEAT_OP,	
            /// <summary>
            /// bad perl operator
            /// </summary>
            CRE2_ERROR_BAD_PERL_OP,
            /// <summary>
            /// invalid UTF-8 in regexp
            /// </summary>
            CRE2_ERROR_BAD_UTF8,
            /// <summary>
            /// bad named capture group
            /// </summary>
            CRE2_ERROR_BAD_NAMED_CAPTURE,
            /// <summary>
            /// pattern too large (compile failed)
            /// <summary>
            CRE2_ERROR_PATTERN_TOO_LARGE,
        }


        /* construction and destruction */
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern cre2_regexp_t_ptr cre2_new(
            byte[] pattern,
            int pattern_len,
			cre2_options_t_ptr opt);

        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_delete(cre2_regexp_t_ptr re);


        /* regular expression inspection */
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_pattern(cre2_regexp_t_ptr re);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern cre2_error_code_t cre2_error_code(cre2_regexp_t_ptr re);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_num_capturing_groups(cre2_regexp_t_ptr re);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_find_named_capturing_groups(
            cre2_regexp_t_ptr re, [MarshalAs(UnmanagedType.LPStr)]string name);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_program_size(cre2_regexp_t_ptr re);


        /* invalidated by further re use */
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_error_string(cre2_regexp_t_ptr re);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_error_arg(cre2_regexp_t_ptr re, [MarshalAs(UnmanagedType.LPStruct)]cre2_string_t arg);
    }
}
