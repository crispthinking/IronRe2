namespace IronRe2;

/// <summary>Safe handle wrapper around <c>cre2_regexp_t_ptr</c></summary>
public sealed class RegexHandle : Re2Handle
{
    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        Re2Ffi.cre2_delete(handle);
        return true;
    }
}
