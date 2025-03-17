using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using cre2_regexp_t_ptr = IronRe2.RegexHandle;
using cre2_set_t_ptr = IronRe2.RegexSetHandle;
using cre2_options_t_ptr = IronRe2.OptionsHandle;
using cre2_named_groups_iter_t_ptr = IronRe2.NamedCaptureIteratorHandle;

namespace IronRe2;

/// <summary>
///     This class contains the P/Invoke definitions for the cre2 library's
///     public interface.
/// </summary>
internal static unsafe partial class Re2Ffi
{
    // Main matching functions.
    public enum cre2_anchor_t
    {
        CRE2_UNANCHORED = 1,
        CRE2_ANCHOR_START = 2,
        CRE2_ANCHOR_BOTH = 3
    }

    // Regular expressions configuration options.
    public enum cre2_encoding_t
    {
        CRE2_UNKNOWN = 0, // should never happen
        CRE2_UTF8 = 1,
        CRE2_Latin1 = 2
    }

    /// <summary>
    ///     This definition  must be  kept in sync  with the definition  of
    ///     "enum ErrorCode" in the file "re2.h" of the original RE2
    ///     distribution.
    /// </summary>
    public enum cre2_error_code_t
    {
        CRE2_NO_ERROR = 0,

        /// <summary>
        ///     unexpected error
        /// </summary>
        CRE2_ERROR_INTERNAL,

        /// <summary>
        ///     bad escape sequence
        /// </summary>
        CRE2_ERROR_BAD_ESCAPE,

        /// <summary>
        ///     bad character class
        /// </summary>
        CRE2_ERROR_BAD_CHAR_CLASS,

        /// <summary>
        ///     bad character class range
        /// </summary>
        CRE2_ERROR_BAD_CHAR_RANGE,

        /// <summary>
        ///     missing closing ]
        /// </summary>
        CRE2_ERROR_MISSING_BRACKET,

        /// <summary>
        ///     missing closing )
        /// </summary>
        CRE2_ERROR_MISSING_PAREN,

        /// <summary>
        ///     trailing \ at end of regexp
        /// </summary>
        CRE2_ERROR_TRAILING_BACKSLASH,

        /// <summary>
        ///     repeat argument missing, e.g. "*"
        /// </summary>
        CRE2_ERROR_REPEAT_ARGUMENT,

        /// <summary>
        ///     bad repetition argument
        /// </summary>
        CRE2_ERROR_REPEAT_SIZE,

        /// <summary>
        ///     bad repetition operator
        /// </summary>
        CRE2_ERROR_REPEAT_OP,

        /// <summary>
        ///     bad perl operator
        /// </summary>
        CRE2_ERROR_BAD_PERL_OP,

        /// <summary>
        ///     invalid UTF-8 in regexp
        /// </summary>
        CRE2_ERROR_BAD_UTF8,

        /// <summary>
        ///     bad named capture group
        /// </summary>
        CRE2_ERROR_BAD_NAMED_CAPTURE,

        /// <summary>
        ///     pattern too large (compile failed)
        /// </summary>
        CRE2_ERROR_PATTERN_TOO_LARGE
    }

    // Version functions.
    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr cre2_version_string();

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr cre2_version_interface_current();

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr cre2_version_interface_revision();

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr cre2_version_interface_age();

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial cre2_options_t_ptr cre2_opt_new();

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_delete(IntPtr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_posix_syntax(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_longest_match(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_log_errors(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_literal(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_never_nl(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_dot_nl(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_never_capture(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_case_sensitive(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_perl_classes(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_word_boundary(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_one_line(cre2_options_t_ptr opt, int flag);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_max_mem(cre2_options_t_ptr opt, [MarshalAs(UnmanagedType.I8)] long m);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_opt_set_encoding(cre2_options_t_ptr opt, cre2_encoding_t enc);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_posix_syntax(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_longest_match(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_log_errors(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_literal(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_never_nl(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_dot_nl(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_never_capture(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_case_sensitive(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_perl_classes(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_word_boundary(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_opt_one_line(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I8)]
    public static partial long cre2_opt_max_mem(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial cre2_encoding_t cre2_opt_encoding(cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial cre2_regexp_t_ptr cre2_new(
        in byte pattern,
        int pattern_len,
        cre2_options_t_ptr opt);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_delete(IntPtr re);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr cre2_pattern(cre2_regexp_t_ptr re);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial cre2_error_code_t cre2_error_code(cre2_regexp_t_ptr re);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_num_capturing_groups(cre2_regexp_t_ptr re);

    // For string parameters, we now specify StringMarshalling = Utf8.
    [LibraryImport("cre2", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_find_named_capturing_groups(cre2_regexp_t_ptr re, string name);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_program_size(cre2_regexp_t_ptr re);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial cre2_named_groups_iter_t_ptr cre2_named_groups_iter_new(cre2_regexp_t_ptr re);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    public static unsafe partial bool cre2_named_groups_iter_next(cre2_named_groups_iter_t_ptr iter, out char* name,
        out int index);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_named_groups_iter_delete(IntPtr iter);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr cre2_error_string(cre2_regexp_t_ptr re);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_error_arg(
        cre2_regexp_t_ptr re,
        ref cre2_string_t arg);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial int cre2_match(cre2_regexp_t_ptr re,
        byte* text, int textlen,
        int startpos, int endpos, cre2_anchor_t anchor,
        cre2_string_t* match, int nmatch);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_easy_match(
        in byte pattern, int pattern_len,
        in byte text, int text_len,
        [Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
        cre2_string_t[] match, int nmatch);

    // Set match.
    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial cre2_set_t_ptr cre2_set_new(cre2_options_t_ptr opt, cre2_anchor_t anchor);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void cre2_set_delete(IntPtr set);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_set_add(
        cre2_set_t_ptr set,
        [In] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
        byte[] pattern, UIntPtr pattern_len,
        [Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
        byte[] error, UIntPtr error_len);

    [LibraryImport("cre2", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_set_add_simple(cre2_set_t_ptr set, string pattern);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int cre2_set_compile(cre2_set_t_ptr set);

    [LibraryImport("cre2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial UIntPtr cre2_set_match(
        cre2_set_t_ptr set, in byte text, UIntPtr text_len,
        [Out] [MarshalAs(UnmanagedType.LPArray)]
        int[] match, UIntPtr match_len);

    // Precompiled regular expressions.
    [StructLayout(LayoutKind.Sequential)]
    public struct cre2_string_t
    {
        public IntPtr data;
        public int length;
    }
}
