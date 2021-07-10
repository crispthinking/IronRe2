namespace IronRe2
{
    public class RegexHandle : Re2Handle
    {
        protected override bool ReleaseHandle()
        {
            Re2Ffi.cre2_delete(handle);
            return true;
        }
    }
}