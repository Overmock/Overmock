using Overmock.Proxies;
using Overmock.Proxies.Internal;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProxyFactoryProvider : IProxyFactoryProvider
    {
        private static readonly IProxyFactoryProvider ProxyFactory = new CachedProxyFactoryProvider();
        private static IProxyFactoryProvider _current = ProxyFactory;

		/// <summary>
		/// 
		/// </summary>
		public static readonly IProxyFactoryProvider Default = ProxyFactory;

		private ProxyFactoryProvider()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static IProxyFactoryProvider Current => _current;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IProxyFactoryProvider Use(IProxyFactoryProvider  factory)
        {
            return Interlocked.Exchange(ref _current, factory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interceptor"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IProxyFactory Proxy(IInterceptor interceptor)
        {
            return ProxyFactory.Provide(interceptor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
		public abstract IProxyFactory Provide(IInterceptor interceptor);

		private class CachedProxyFactoryProvider : ProxyFactoryProvider
		{
            private readonly IProxyFactory _interfaceProxyFactory = new InterfaceProxyFactory(GeneratedProxyCache.Cache);
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