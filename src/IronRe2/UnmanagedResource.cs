using System;

namespace IronRe2;

/// <summary>Represents a wrapper around a raw unmanaged resource.</summary>
public abstract class UnmanagedResource<T> : IDisposable
    where T : Re2Handle
{
    // Raw handle to the underlying unmanaged resource

    /// <summary>
    ///     Initialise the unmanaged resource with the given
    ///     <paramref ref="rawHandle" />.
    /// </summary>
    /// <param name="rawHandle">The handle for this resource.</param>
    protected UnmanagedResource(T rawHandle)
    {
        RawHandle = rawHandle;
    }

    /// <summary>
    ///     Get the handle to the underlying resource
    /// </summary>
    internal T RawHandle { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        RawHandle.Dispose();
        GC.SuppressFinalize(this);
    }
}
