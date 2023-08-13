namespace Kimono.Internal
{
	/// <summary>
	/// Class ProxyGenerator.
	/// Implements the <see cref="Kimono.IProxyGenerator{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.IProxyGenerator{T}" />
	internal sealed class ProxyGenerator<T> : IProxyGenerator<T> where T : class
	{
		/// <summary>
		/// The proxy context
		/// </summary>
		private readonly ProxyContext _proxyContext;
		/// <summary>
		/// The dynamic type
		/// </summary>
		private readonly Type _dynamicType;
		/// <summary>
		/// The create proxy
		/// </summary>
		private readonly Func<ProxyContext, IInterceptor, Type, object> _createProxy;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyGenerator{T}"/> class.
		/// </summary>
		/// <param name="proxyContext">The proxy context.</param>
		/// <param name="dynamicType">Type of the dynamic.</param>
		/// <param name="createProxy">The create proxy.</param>
		public ProxyGenerator(ProxyContext proxyContext, Type dynamicType, Func<ProxyContext, IInterceptor, Type, object> createProxy)
		{
			_proxyContext = proxyContext;
			_dynamicType = dynamicType;
			_createProxy = createProxy;
		}

		/// <summary>
		/// Generates the proxy.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>System.Object.</returns>
		public object GenerateProxy(IInterceptor interceptor)
		{
			return _createProxy.Invoke(_proxyContext, interceptor, _dynamicType);
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