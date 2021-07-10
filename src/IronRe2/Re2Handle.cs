using System;
using System.Runtime.InteropServices;

namespace IronRe2
{
    public abstract class Re2Handle : SafeHandle
    {
        protected Re2Handle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;
    }
}