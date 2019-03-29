using System.Runtime.InteropServices;

namespace IronRe2
{
    public static class Meta
    {
        /// <summary>
        /// Get the version string from the native code
        /// </summary>
        public static string VersionString => Marshal.PtrToStringAnsi(Re2Ffi.cre2_version_string());

        /// <summary>
        ///  Get the Numerical version, packed as a tuple.
        /// </summary>
        public static (int age, int current, int revision) Version => (
            (int)Re2Ffi.cre2_version_interface_age(),
            (int)Re2Ffi.cre2_version_interface_current(),
            (int)Re2Ffi.cre2_version_interface_revision()
        );
    }
}
