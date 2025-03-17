using System.Runtime.InteropServices;

namespace IronRe2;

/// <summary>
///     Library version information.
/// </summary>
public static class Meta
{
    /// <summary>
    ///     Get the version string from the native code
    /// </summary>
    public static string? VersionString =>
        Marshal.PtrToStringAnsi(Re2Ffi.cre2_version_string());

    /// <summary>
    ///     Get the Numerical version, packed as a tuple.
    /// </summary>
    public static (int current, int revision, int age) Version => (
        (int)Re2Ffi.cre2_version_interface_current(),
        (int)Re2Ffi.cre2_version_interface_revision(),
        (int)Re2Ffi.cre2_version_interface_age()
    );
}
