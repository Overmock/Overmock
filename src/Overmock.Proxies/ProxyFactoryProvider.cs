using Overmock.Proxies;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyFactoryProvider : IProxyFactoryProvider
    {
        private static readonly IProxyFactoryProvider ProxyFactory = new ProxyMarshallerFactory();
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
            return ProxyFactory.Create(interceptor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
		public IProxyFactory Create(IInterceptor interceptor)
		{
			return Current.Create(interceptor);
		}
	}
}