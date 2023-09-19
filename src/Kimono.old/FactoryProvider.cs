using Kimono.Internal;
using Kimono.Proxies;
using Kimono.Proxies.Internal;
using System;
using System.Threading;

namespace Kimono
{
    /// <summary>
    /// Class ProxyFactoryProvider.
    /// Implements the <see cref="IFactoryProvider" />
    /// </summary>
    /// <seealso cref="IFactoryProvider" />
    public abstract class FactoryProvider : IFactoryProvider
    {
        //private static IDelegateFactory _delegateFactory = new ExpressionDelegateFactory();
        private static IDelegateFactory _delegateFactory = new DynamicMethodDelegateFactory();
        private static IFactoryProvider _proxyFactory = new CachedProxyFactoryProvider();

        /// <summary>
        /// The default <see cref="IFactoryProvider" />
        /// </summary>
        public static readonly IFactoryProvider ProxyFactory = _proxyFactory;

        /// <summary>
        /// Prevents a default instance of the <see cref="FactoryProvider"/> class from being created.
        /// </summary>
        private FactoryProvider()
        {
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static IDelegateFactory DelegateFactory => _delegateFactory;

        /// <summary>
        /// Uses the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>IProxyFactoryProvider.</returns>
        public static IFactoryProvider Use(IFactoryProvider factory)
        {
            return Interlocked.Exchange(ref _proxyFactory, factory);
        }

        /// <summary>
        /// Uses the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>IProxyFactoryProvider.</returns>
        public static IDelegateFactory Use(IDelegateFactory factory)
        {
            Interlocked.Exchange(ref _delegateFactory, factory);
            
            return _delegateFactory;
        }

        /// <summary>
        /// Configures the <see cref="ProxyFactory"/> to use Expressions to compile delegates.
        /// </summary>
        /// <returns></returns>
        public static IDelegateFactory UseExpressionDelegates()
        {
            if (_delegateFactory is DynamicMethodDelegateFactory)
            {
                Use(new ExpressionDelegateFactory());
            }

            return _delegateFactory;
        }

        /// <summary>
        /// Configures the <see cref="ProxyFactory"/> to use Expressions to compile delegates.
        /// </summary>
        /// <returns></returns>
        public static IDelegateFactory UseDynamicMethodDelegates()
        {
            if (_delegateFactory is ExpressionDelegateFactory)
            {
                Use(new DynamicMethodDelegateFactory());
            }

            return _delegateFactory;
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
        /// Implements the <see cref="FactoryProvider" />
        /// </summary>
        /// <seealso cref="FactoryProvider" />
        private sealed class CachedProxyFactoryProvider : FactoryProvider
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