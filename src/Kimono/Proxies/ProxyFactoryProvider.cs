using Kimono.Internal;

namespace Kimono.Proxies
{
    /// <summary>
    /// Class ProxyFactoryProvider.
    /// Implements the <see cref="IProxyFactoryProvider" />
    /// </summary>
    /// <seealso cref="IProxyFactoryProvider" />
    public abstract class ProxyFactoryProvider : IProxyFactoryProvider
    {
        private static readonly IProxyFactoryProvider _proxyFactory = new CachedProxyFactoryProvider();
        private static IProxyFactoryProvider _current = _proxyFactory;

        /// <summary>
        /// The default <see cref="IProxyFactoryProvider" />
        /// </summary>
        public static readonly IProxyFactoryProvider Default = _proxyFactory;

        /// <summary>
        /// Prevents a default instance of the <see cref="ProxyFactoryProvider"/> class from being created.
        /// </summary>
        private ProxyFactoryProvider()
        {
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static IProxyFactoryProvider Current => _current;

        /// <summary>
        /// Uses the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>IProxyFactoryProvider.</returns>
        public static IProxyFactoryProvider Use(IProxyFactoryProvider factory)
        {
            return Interlocked.Exchange(ref _current, factory);
        }

        /// <summary>
        /// Proxies the specified interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>IProxyFactory.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IProxyFactory Proxy(IInterceptor interceptor)
        {
            return _proxyFactory.Provide(interceptor);
        }

        /// <summary>
        /// Provides the specified interceptor.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>IProxyFactory.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public abstract IProxyFactory Provide(IInterceptor interceptor);

        /// <summary>
        /// Class CachedProxyFactoryProvider.
        /// Implements the <see cref="ProxyFactoryProvider" />
        /// </summary>
        /// <seealso cref="ProxyFactoryProvider" />
        private sealed class CachedProxyFactoryProvider : ProxyFactoryProvider
        {
            /// <summary>
            /// The interface proxy factory
            /// </summary>
            private readonly IProxyFactory _interfaceProxyFactory = new InterfaceProxyFactory(GeneratedProxyCache.Cache);
            /// <summary>
            /// Provides the specified interceptor.
            /// </summary>
            /// <param name="interceptor">The interceptor.</param>
            /// <returns>IProxyFactory.</returns>
            /// <exception cref="NotImplementedException"></exception>
            public override IProxyFactory Provide(IInterceptor interceptor)
            {
                if (interceptor.IsInterface())
                {
                    return _interfaceProxyFactory;
                }

                if (interceptor.IsDelegate())
                {
                    return new DelegateProxyFactory(GeneratedProxyCache.Cache);
                }

                throw new NotImplementedException();
            }
        }
    }
}