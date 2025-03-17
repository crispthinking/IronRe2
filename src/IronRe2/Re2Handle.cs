using System;
using System.Runtime.InteropServices;

namespace IronRe2;

/// <summary>
///     Base <see cref="SafeHandle" /> implementation for Re2 pointers. This
///     is used to ensure that unmanaged Re2 objects are cleaned up when they
///     are no longer needed, even when leaked.
/// </summary>
public abstract class Re2Handle : SafeHandle
{
    /// <summary>Create a new handle instance, initialised with the default value</summary>
    protected Re2Handle()
        : base(IntPtr.Zero, true)
    {
    }

    /// <summary>Returns true if the handle holds an invalid pointer.</summary>
    public override bool IsInvalid => handle == IntPtr.Zero;
}
