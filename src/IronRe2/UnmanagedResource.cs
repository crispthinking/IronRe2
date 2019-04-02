using System;
using System.Threading;

namespace IronRe2
{
    public abstract class UnmanagedResource : IDisposable
    {
        
        // Raw handle to the unmanaged regex object
        private IntPtr _rawHandle;

        protected UnmanagedResource(IntPtr rawHandle)
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
        internal IntPtr RawHandle => _rawHandle;

        /// <summary>
        ///  Free the resource
        /// </summary>
        /// <param name="handle">The handle to free</param>
        protected abstract void Free(IntPtr handle);

        
        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            var handle = Interlocked.Exchange(ref _rawHandle, IntPtr.Zero);
            if (handle != IntPtr.Zero)
            {
                Re2Ffi.cre2_delete(_rawHandle);
            }
            GC.SuppressFinalize(this);
        }
    }
}
