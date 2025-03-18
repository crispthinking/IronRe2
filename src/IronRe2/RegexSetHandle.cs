namespace IronRe2;

/// <summary>Safe handle wrapper around <c>cre2_set_t_ptr</c></summary>
public sealed class RegexSetHandle : Re2Handle
{
    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        Re2Ffi.cre2_set_delete(handle);
        return true;
    }
}
