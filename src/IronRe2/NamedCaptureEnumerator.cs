using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IronRe2;

/// <summary>
///     Enumerator for walking an interator of named capture groups.
/// </summary>
internal class NamedCaptureEnumerator : UnmanagedResource<NamedCaptureIteratorHandle>,
    IEnumerator<NamedCaptureGroup>
{
    private NamedCaptureGroup? _current;

    public NamedCaptureEnumerator(Regex regex)
        : base(Re2Ffi.cre2_named_groups_iter_new(regex.RawHandle))
    {
    }

    /// <summary>
    ///     The current named capture this enumerator is pointing at.
    /// </summary>
    /// <value>
    ///     Named capture information, or null if the enumerator isn't pointing
    ///     at a valid item.
    /// </value>
    public NamedCaptureGroup Current => _current ?? throw new InvalidOperationException();

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <summary>
    ///     Advance the enumerator
    /// </summary>
    /// <returns>
    ///     True if <see cref="Current" /> now points to a valid
    ///     <see cref="NamedCaptureGroup" />
    /// </returns>
    public unsafe bool MoveNext()
    {
        if (Re2Ffi.cre2_named_groups_iter_next(RawHandle, out var namePtr, out var index))
        {
            var name = Marshal.PtrToStringAnsi(new IntPtr(namePtr));
            if (name != null)
            {
                _current = new NamedCaptureGroup(name, index);
            }

            return true;
        }

        _current = null;
        return false;
    }

    /// <summary>
    ///     Resetting this enumerator isn't supported
    /// </summary>
    public void Reset()
    {
        throw new NotSupportedException();
    }
}
