using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

using cre2_regexp_t_ptr = System.IntPtr;
using cre2_set_t_ptr = System.IntPtr;
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

        public enum cre2_encoding_t {
           CRE2_UNKNOWN  = 0,    /* should never happen */
           CRE2_UTF8 = 1,
           CRE2_Latin1   = 2
        }


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern  cre2_options_t_ptr cre2_opt_new();
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern  void cre2_opt_delete(cre2_options_t_ptr opt);

        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_posix_syntax    (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_longest_match   (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_log_errors      (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_literal     (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_never_nl        (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_dot_nl      (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_never_capture   (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_case_sensitive  (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_perl_classes    (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_word_boundary   (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_one_line        (cre2_options_t_ptr opt, int flag);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_max_mem     (cre2_options_t_ptr opt, [MarshalAs(UnmanagedType.I8)]long m);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void cre2_opt_set_encoding        (cre2_options_t_ptr opt, cre2_encoding_t enc);
     
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_posix_syntax     (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_longest_match        (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_log_errors       (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_literal          (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_never_nl         (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_dot_nl           (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_never_capture        (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_case_sensitive       (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_perl_classes     (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_word_boundary        (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_opt_one_line         (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.I8)]
        public static extern long cre2_opt_max_mem      (cre2_options_t_ptr opt);
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern cre2_encoding_t cre2_opt_encoding (cre2_options_t_ptr opt);

        /** --------------------------------------------------------------------
         ** Precompiled regular expressions.
         ** ----------------------------------------------------------------- */

        [StructLayout(LayoutKind.Sequential)]
        public struct cre2_string_t {
            public IntPtr data;
            public int length;
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
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]byte[] pattern,
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
        public static extern void cre2_error_arg(
            cre2_regexp_t_ptr re,
            [In, Out]ref cre2_string_t arg);


        /** --------------------------------------------------------------------
         ** Main matching functions.
         ** ----------------------------------------------------------------- */

         public enum cre2_anchor_t {
            CRE2_UNANCHORED   = 1,
            CRE2_ANCHOR_START = 2,
            CRE2_ANCHOR_BOTH  = 3
        }

        /// <returns>
        /// 0  for  no  match, 1 for  successful matching
        /// </returns>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_match(cre2_regexp_t_ptr re,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]byte[] text, int textlen,
            int startpos, int endpos, cre2_anchor_t anchor,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=6)]cre2_string_t[] match, int nmatch);

        /// <returns>
        /// 0  for  no  match, 1 for  successful
        /// matching, 2 for wrong regexp
        /// </returns>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_easy_match(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]byte[] pattern, int pattern_len,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)]byte[] text, int text_len,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)]cre2_string_t[] match, int nmatch);


        /** --------------------------------------------------------------------
         ** Set match.
         ** ----------------------------------------------------------------- */

        /// <summary>
        /// RE2::Set constructor
        /// </summary>
        
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern cre2_set_t_ptr cre2_set_new(cre2_options_t_ptr opt, cre2_anchor_t anchor);
        /// <summary>
        /// RE2::Set destructor
        /// </summary>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern void      cre2_set_delete(cre2_set_t_ptr set);
        /// <summary>
        /// Add a regex to the set. If invalid: store error message in error buffer.
        /// Returns the index associated to this regex, -1 on error
        /// </summary>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_set_add(
            cre2_set_t_ptr set,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)]byte[] pattern, UIntPtr pattern_len,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)]byte[] error, UIntPtr error_len);
        /// <summary>
        /// Add pattern without NULL byte. Discard error message.
        /// Returns the index associated to this regex, -1 on error
        /// </summary>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_set_add_simple(
            cre2_set_t_ptr set, [MarshalAs(UnmanagedType.LPStr)]string pattern);
        /// <summary>
        /// Compile the regex set into a DFA. Must be called after add and before match.
        /// Returns 1 on success, 0 on error
        /// </summary>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern int cre2_set_compile(cre2_set_t_ptr set);
        /// <summary>
        /// Match the set of regex against text and store indices of matching regexes in match array.
        /// Returns the number of regexes which match.
        /// </summary>
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern UIntPtr cre2_set_match(
            cre2_set_t_ptr set, byte[] text, UIntPtr text_len,
            [Out, MarshalAs(UnmanagedType.LPArray)]int[] match, UIntPtr match_len);
    }
}
