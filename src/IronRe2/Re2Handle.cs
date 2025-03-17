using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace IronRe2;

/// <summary>
/// Base <see cref="SafeHandle"/> implementation for Re2 pointers. This
/// is used to ensure that unmanaged Re2 objects are cleaned up when they
/// are no longer needed, even when leaked.
/// </summary>
public abstract class Re2Handle : SafeHandle
{
    /// <summary>
    /// Create a new handle instance, initialised with the default value.
    /// </summary>
    protected Re2Handle()
        : base(IntPtr.Zero, true)
    {
        // Log creation of a new instance for debugging purposes.
        Debug.WriteLine($"[Re2Handle] Created instance with handle: {handle}");
    }

    /// <summary>
    /// Returns true if the handle holds an invalid pointer.
    /// </summary>
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    /// Derived classes implement this to release the unmanaged resource.
    /// </summary>
    protected abstract bool ReleaseHandleCore();

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        // Log that we're about to release a handle.
        if (!IsInvalid)
        {
            Debug.WriteLine($"[Re2Handle] Releasing handle: {handle}");
            try
            {
                var result = ReleaseHandleCore();
                return result;
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                Debug.WriteLine($"[Re2Handle] Exception during ReleaseHandle: {ex}");
                // Returning false indicates that the release failed.
                return false;
            }
        }

        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
        Debug.WriteLine("[Re2Handle] Attempted to release an invalid handle.");
        return true;
    }
}
