using Overmock.Proxies;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class MarshallerFactory : IMarshallerFactory
    {
        private static readonly IMarshallerFactory ProxyFactory = new ProxyMarshallerFactory();
        private static IMarshallerFactory _current = ProxyFactory;

		/// <summary>
		/// 
		/// </summary>
		public static readonly IMarshallerFactory Default = ProxyFactory;

		private MarshallerFactory()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static IMarshallerFactory Current => _current;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IMarshallerFactory Use(IMarshallerFactory  factory)
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
        public static IMarshaller Proxy(IInterceptor interceptor)
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
		public IMarshaller Create(IInterceptor interceptor)
		{
			return Current.Create(interceptor);
		}
	}
}