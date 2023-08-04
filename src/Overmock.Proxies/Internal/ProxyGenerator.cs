namespace Overmock.Proxies.Internal
{
    internal class ProxyGenerator<T> : IProxyGenerator<T> where T : class
	{
		private readonly ProxyContext _proxyContext;
		private readonly Type _dynamicType;
		private readonly Func<ProxyContext, IInterceptor, Type, object> _createProxy;

		public ProxyGenerator(ProxyContext proxyContext, Type dynamicType, Func<ProxyContext, IInterceptor, Type, object> createProxy)
		{
			_proxyContext = proxyContext;
			_dynamicType = dynamicType;
			_createProxy = createProxy;
		}

		public object GenerateProxy(IInterceptor interceptor)
		{
			return _createProxy.Invoke(_proxyContext, interceptor, _dynamicType);
		}

		public T GenerateProxy(IInterceptor<T> interceptor)
		{
			return (T)GenerateProxy((IInterceptor)interceptor);
		}
	}
}