using System;

namespace IronRe2
{
    /// <summary>Represents a wrapper around a raw unmanaged resource.</summary>
    public abstract class UnmanagedResource<T> : IDisposable
        where T : Re2Handle
    {
        private T _rawHandle;
        private bool _disposed;

        /// <summary>
        /// Initialize the unmanaged resource with the given raw handle.
        /// </summary>
        /// <param name="rawHandle">The handle for this resource.</param>
        protected UnmanagedResource(T rawHandle)
        {
            _rawHandle = rawHandle ?? throw new ArgumentNullException(nameof(rawHandle));
        }

        /// <summary>
        /// Gets the handle to the underlying resource.
        /// Throws ObjectDisposedException if the resource has been disposed.
        /// </summary>
        internal T RawHandle
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return _rawHandle;
            }
        }

        /// <summary>
        /// Finalizer in case Dispose is not called explicitly.
        /// Throws ObjectDisposedException if the resource has been disposed.
        /// </summary>
        ~UnmanagedResource() => Dispose(false);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of unmanaged (and optionally managed) resources.
        /// </summary>
        /// <param name="disposing">True if called explicitly; false if from the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // If disposing equals true, dispose managed resources.
                if (disposing)
                {
                    // Dispose managed objects here if needed.
                }

                // Always dispose the underlying unmanaged resource.
                if (_rawHandle != null)
                {
                    _rawHandle.Dispose();
                }

                _disposed = true;
            }
        }
    }
}

