/* Unmerged change from project 'Kimono (netstandard2.1)'
Before:
using Kimono.Internal;
After:
using Kimono;
using Kimono;
using Kimono.Internal;
*/
using Kimono.Internal;
using Kimono.Proxies;
using System;
using System.Threading;

namespace Kimono
{
    /// <summary>
    /// Class ProxyFactoryProvider.
    /// Implements the <see cref="IProxyFactoryProvider" />
    /// </summary>
    /// <seealso cref="IProxyFactoryProvider" />
    public abstract class ProxyFactoryProvider : IProxyFactoryProvider
    {
        private static IMethodDelegateFactory _delegateFactory = new ExpressionMethodDelegateGenerator();
        private static IProxyFactoryProvider _proxyFactory = new CachedProxyFactoryProvider();

        /// <summary>
        /// The default <see cref="IProxyFactoryProvider" />
        /// </summary>
        public static readonly IProxyFactoryProvider ProxyFactory = _proxyFactory;

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
        public static IMethodDelegateFactory DelegateFactory => _delegateFactory;

        /// <summary>
        /// Uses the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>IProxyFactoryProvider.</returns>
        public static IProxyFactoryProvider Use(IProxyFactoryProvider factory)
        {
            return Interlocked.Exchange(ref _proxyFactory, factory);
        }

        /// <summary>
        /// Uses the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>IProxyFactoryProvider.</returns>
        public static IMethodDelegateFactory Use(IMethodDelegateFactory factory)
        {
            return Interlocked.Exchange(ref _delegateFactory, factory);
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

                throw new NotImplementedException();
            }
        }
    }
}