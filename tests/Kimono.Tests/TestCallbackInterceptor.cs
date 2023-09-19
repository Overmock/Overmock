namespace Kimono.Tests
{
    internal class TestCallbackInterceptor<T> : Interceptor<T> where T : class
    {
        private readonly Action<IInvocation> _callback;

        public TestCallbackInterceptor(T target = null, Action<IInvocation> callback = null) : base(target)
        {
            _callback = callback;
        }

        protected override void HandleInvocation(IInvocation invocation)
        {
            _callback.Invoke(invocation);
        }
    }

    internal sealed class DisposableTestCallbackInterceptor<T> : DisposableInterceptor<T>, IDisposable where T : class, IDisposable
    {
        private readonly Action<IInvocation> _callback;

        public DisposableTestCallbackInterceptor(T target, Action<IInvocation> callback) : base(target)
        {
            _callback = callback;
        }

        protected override void HandleInvocation(IInvocation invocation)
        {
            _callback.Invoke(invocation);
        }

        protected override void Disposing(bool disposing)
        {
            if (disposing)
            {
                Target?.Dispose();
            }
        }
    }
}
