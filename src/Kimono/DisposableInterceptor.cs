using System;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DisposableInterceptor<T> : Interceptor<T>, IDisposableInterceptor<T> where T : class, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        public DisposableInterceptor(T target) : base(target)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        ~DisposableInterceptor()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Disposing(disposing);

                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected abstract void Disposing(bool disposing);

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}