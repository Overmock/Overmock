using Kimono.Proxies;

namespace Kimono.Internal
{
    /// <summary>
    /// Class ProxyGenerator.
    /// Implements the <see cref="IProxyGenerator{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IProxyGenerator{T}" />
    internal sealed class ProxyGenerator<T> : IProxyGenerator<T> where T : class
	{
		private readonly Type _proxyType;
		private readonly ProxyContext _proxyContext;
		private readonly Func<ProxyContext, IInterceptor, Type, object> _createProxy;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyGenerator{T}"/> class.
		/// </summary>
		/// <param name="proxyType">Type of the proxy.</param>
		/// <param name="proxyContext">The proxy context.</param>
		/// <param name="createProxy">The create proxy delegate.</param>
		public ProxyGenerator(Type proxyType, ProxyContext proxyContext, Func<ProxyContext, IInterceptor, Type, object> createProxy)
		{
			_proxyContext = proxyContext;
			_proxyType = proxyType;
			_createProxy = createProxy;
		}

		/// <summary>
		/// Generates the proxy.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>System.Object.</returns>
		public object GenerateProxy(IInterceptor interceptor)
		{
			return _createProxy.Invoke(_proxyContext, interceptor, _proxyType);
		}

		/// <summary>
		/// Generates the proxy.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>T.</returns>
		public T GenerateProxy(IInterceptor<T> interceptor)
		{
			return (T)GenerateProxy((IInterceptor)interceptor);
		}
	}
}