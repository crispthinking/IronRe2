namespace IronRe2;

internal sealed class NamedCaptureIteratorHandle : Re2Handle
{
    protected override bool ReleaseHandleCore()
    {
        Re2Ffi.cre2_named_groups_iter_delete(handle);
        return true;
    }
}
