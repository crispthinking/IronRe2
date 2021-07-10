namespace IronRe2
{
    public class RegexSetHandle : Re2Handle
    {
        protected override bool ReleaseHandle()
        {
            Re2Ffi.cre2_set_delete(handle);
            return true;
        }
    }
}