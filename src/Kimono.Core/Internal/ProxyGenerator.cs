using System;

namespace Kimono.Core.Internal
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
        private readonly Func<IInterceptor<T>, T> _createProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyGenerator{T}"/> class.
        /// </summary>
        /// <param name="proxyContext">The proxy context.</param>
        /// <param name="createProxy">The create proxy delegate.</param>
        public ProxyGenerator(ProxyContext proxyContext, Func<IInterceptor<T>, T> createProxy)
        {
            _proxyContext = proxyContext;
            _createProxy = createProxy;
        }

        /// <summary>
        /// Generates the proxy.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>The generated proxy.</returns>
        object IProxyGenerator.GenerateProxy(IInterceptor interceptor)
        {
            return (T)GenerateProxy((IInterceptor<T>)interceptor);
        }

        /// <summary>
        /// Generates the proxy.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns><typeparamref name="T" />.</returns>
        public T GenerateProxy(IInterceptor<T> interceptor)
        {
            return _createProxy.Invoke(interceptor);
        }
    }
}