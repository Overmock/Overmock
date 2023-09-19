using System;

namespace Kimono.Proxies.Internal
{
    /// <summary>
    /// Class ProxyGenerator.
    /// Implements the <see cref="IProxyGenerator{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IProxyGenerator{T}" />
    internal sealed class ProxyGenerator<T> : IProxyGenerator<T> where T : class
    {
        private readonly ProxyContext _proxyContext;
        private readonly Func<ProxyContext, IInterceptor, T> _createProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyGenerator{T}"/> class.
        /// </summary>
        /// <param name="proxyContext">The proxy context.</param>
        /// <param name="createProxy">The create proxy delegate.</param>
        public ProxyGenerator(ProxyContext proxyContext, Func<ProxyContext, IInterceptor, T> createProxy)
        {
            _proxyContext = proxyContext;
            _createProxy = createProxy;
        }

        /// <summary>
        /// Generates the proxy.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>The generated proxy.</returns>
        public object GenerateProxy(IInterceptor interceptor)
        {
            return _createProxy.Invoke(_proxyContext, interceptor);
        }

        /// <summary>
        /// Generates the proxy.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns><typeparamref name="T" />.</returns>
        public T GenerateProxy(IInterceptor<T> interceptor)
        {
            return (T)GenerateProxy((IInterceptor)interceptor);
        }
    }
}