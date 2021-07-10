using System;
using System.Threading;

namespace IronRe2
{
    public abstract class UnmanagedResource<T> : IDisposable
        where T: Re2Handle
    {

        // Raw handle to the underlying unmanaged resource
        private T _rawHandle;

        protected UnmanagedResource(T rawHandle)
        {
            _rawHandle = rawHandle;
        }

        ~UnmanagedResource()
        {
            Dispose(false);
        }

        /// <summary>
        /// Get the handle to the underlying resource
        /// </summary>
        internal T RawHandle => _rawHandle;
        
        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            _rawHandle.Dispose();
        }
    }
}
