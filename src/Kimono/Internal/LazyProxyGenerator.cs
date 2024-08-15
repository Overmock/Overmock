using System;

namespace Kimono.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyProxyGenerator<T> : IProxyGenerator<T> where T : class
    {
        private readonly Lazy<IProxyGenerator> _lazy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public LazyProxyGenerator(Func<IProxyGenerator> factory)
            => _lazy = new Lazy<IProxyGenerator>(factory);

        /// <inheritdoc />
        public T GenerateProxy(IInterceptor<T> interceptor)
            => ((IProxyGenerator<T>)_lazy.Value).GenerateProxy(interceptor);

        /// <inheritdoc />
        public object GenerateProxy(IInterceptor interceptor)
            => GenerateProxy((IInterceptor<T>)interceptor);
    }
}