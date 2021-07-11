namespace IronRe2
{
    public sealed class RegexHandle : Re2Handle
    {
        protected override bool ReleaseHandle()
        {
            Re2Ffi.cre2_delete(handle);
            return true;
        }
    }
}