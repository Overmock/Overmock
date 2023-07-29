using Overmock.Runtime.Proxies;

namespace Overmock.Runtime
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
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IMarshaller Proxy(IOvermock target, Action<SetupArgs>? argsProvider = null)
        {
            return ProxyFactory.Create(target, argsProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
		public IMarshaller Create(IOvermock target, Action<SetupArgs>? argsProvider = null)
		{
			return Current.Create(target, argsProvider);
		}
	}
}