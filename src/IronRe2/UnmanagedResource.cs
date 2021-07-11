using System;
using System.Threading;

namespace IronRe2
{
    /// <summary>Represents a wrapper around a raw unmanaged resource.</summary>
    public abstract class UnmanagedResource<T> : IDisposable
        where T: Re2Handle
    {

        // Raw handle to the underlying unmanaged resource
        private T _rawHandle;

        /// <summary>
        ///   Initialise the unmanaged resource with the given
        ///   <paramref ref="rawHandle" />.
        /// </summary>
        /// <param name="rawHandle">The handle for this resource.</param>
        protected UnmanagedResource(T rawHandle)
        {
            _rawHandle = rawHandle;
        }

        /// <summary>
        /// Get the handle to the underlying resource
        /// </summary>
        internal T RawHandle => _rawHandle;
        
        /// <inheritdoc />
        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            _rawHandle.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
