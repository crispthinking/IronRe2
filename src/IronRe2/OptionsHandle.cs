namespace IronRe2
{
    public class OptionsHandle : Re2Handle
    {
        protected override bool ReleaseHandle()
        {
            Re2Ffi.cre2_opt_delete(handle);
            return true;
        }
    }
}