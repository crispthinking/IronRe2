namespace IronRe2;

/// <summary>Safe handle wrapper around <c>cre2_options_t_ptr</c></summary>
public sealed class OptionsHandle : Re2Handle
{
    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        Re2Ffi.cre2_opt_delete(handle);
        return true;
    }
}
