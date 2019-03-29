using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace IronRe2
{
    /// <summary>
    /// This class contains the P/Invoke definitions for the cre2 library's
    /// public interface.
    /// </summary>
    class Re2Ffi
    {
        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_string();


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_interface_current();


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_interface_revision();


        [DllImport("cre2", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr cre2_version_interface_age();
    }
}
