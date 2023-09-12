
using System;

namespace Kimono.Core
{
    public abstract class ProxyBase : IProxy
    {
        protected ProxyBase() { }

        protected abstract object? HandleMethodCall(int methodId, Type[] genericParameters, object[] parameters);

        protected abstract void HandleDisposeCall();
    }

    public abstract class ProxyBase<T> : ProxyBase, IProxy<T>
    {
        private readonly IInterceptor<T> _interceptor;

        protected ProxyBase(IInterceptor<T> interceptor)
        {
            _interceptor = interceptor;
        }

        public IInterceptor<T> Interceptor => _interceptor;

        protected sealed override object? HandleMethodCall(int methodId, Type[] genericParameters, object[] parameters)
        {
            return _interceptor.HandleInvocation(methodId, genericParameters, parameters);
        }

        protected sealed override void HandleDisposeCall()
        {
            if (_interceptor is IDisposable dispose)
            {
                dispose.Dispose();
            }
        }
    }
}