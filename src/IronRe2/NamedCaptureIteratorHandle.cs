namespace IronRe2
{
    public class NamedCaptureIteratorHandle : Re2Handle
    {
        protected override bool ReleaseHandle()
        {
            Re2Ffi.cre2_named_groups_iter_delete(handle);
            return true;
        }
    }
}