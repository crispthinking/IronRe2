namespace IronRe2;

/// <summary>
///     Regex syntax and behaviour options.
/// </summary>
public class Options : UnmanagedResource<OptionsHandle>
{
    /// <summary>
    ///     Create a new <see cref="Options" /> instance with the defaults.
    /// </summary>
    public Options()
        : base(Re2Ffi.cre2_opt_new())
    {
    }

    /// <summary>
    ///     restrict regexps to POSIX egrep syntax
    /// </summary>
    public bool PosixSyntax
    {
        get => Re2Ffi.cre2_opt_posix_syntax(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_posix_syntax(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     search for longest match, not first match
    /// </summary>
    public bool LongestMatch
    {
        get => Re2Ffi.cre2_opt_longest_match(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_longest_match(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     log syntax and execution errors to ERROR
    /// </summary>
    public bool LogErrors
    {
        get => Re2Ffi.cre2_opt_log_errors(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_log_errors(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     interpret string as literal, not regexp
    /// </summary>
    public bool Literal
    {
        get => Re2Ffi.cre2_opt_literal(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_literal(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     never match \n, even if it is in regexp
    /// </summary>
    public bool NeverNewline
    {
        get => Re2Ffi.cre2_opt_never_nl(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_never_nl(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     dot matches everything including new line
    /// </summary>
    public bool DotNewline
    {
        get => Re2Ffi.cre2_opt_dot_nl(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_dot_nl(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     parse all parens as non-capturing
    /// </summary>
    public bool NeverCapture
    {
        get => Re2Ffi.cre2_opt_never_capture(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_never_capture(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     match is case-sensitive
    ///     <para>
    ///         regexp can override with (?i) unless in posix_syntax mode
    ///     </para>
    /// </summary>
    public bool CaseSensitive
    {
        get => Re2Ffi.cre2_opt_case_sensitive(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_case_sensitive(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     allow Perl's \d \s \w \D \S \W
    /// </summary>
    public bool PerlClasses
    {
        get => Re2Ffi.cre2_opt_perl_classes(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_perl_classes(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     allow Perl's \b \B (word boundary and not)
    /// </summary>
    public bool WordBoundary
    {
        get => Re2Ffi.cre2_opt_word_boundary(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_word_boundary(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     ^ and $ only match beginning and end of text
    /// </summary>
    public bool OneLine
    {
        get => Re2Ffi.cre2_opt_one_line(RawHandle) != 0;
        set => Re2Ffi.cre2_opt_set_one_line(RawHandle, value ? 1 : 0);
    }

    /// <summary>
    ///     Regex memory budget
    ///     <para>
    ///         The RE2 memory budget is statically divided between the two
    ///         Progs and then the DFAs: two thirds to the forward Prog
    ///         and one third to the reverse Prog.  The forward Prog gives half
    ///         of what it has left over to each of its DFAs.  The reverse Prog
    ///         gives it all to its longest-match DFA.
    ///     </para>
    ///     <para>
    ///         Once a DFA fills its budget, it flushes its cache and starts over.
    ///         If this happens too often, RE2 falls back on the NFA implementation.
    ///     </para>
    /// </summary>
    public long MaxMemory
    {
        get => Re2Ffi.cre2_opt_max_mem(RawHandle);
        set => Re2Ffi.cre2_opt_set_max_mem(RawHandle, value);
    }

    /// <summary>
    ///     text and pattern are UTF-8; otherwise Latin-1
    /// </summary>
    public RegexEncoding Encoding
    {
        get =>
            Re2Ffi.cre2_opt_encoding(RawHandle) switch
            {
                Re2Ffi.cre2_encoding_t.CRE2_Latin1 => RegexEncoding.Latin1,
                Re2Ffi.cre2_encoding_t.CRE2_UTF8 or Re2Ffi.cre2_encoding_t.CRE2_UNKNOWN => RegexEncoding.Utf8,
                _ => RegexEncoding.Utf8
            };
        set => Re2Ffi.cre2_opt_set_encoding(
            RawHandle,
            value == RegexEncoding.Latin1 ? Re2Ffi.cre2_encoding_t.CRE2_Latin1 : Re2Ffi.cre2_encoding_t.CRE2_UTF8);
    }
}
