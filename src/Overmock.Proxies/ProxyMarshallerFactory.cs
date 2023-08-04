using Overmock.Proxies.Internal;

namespace Overmock.Proxies
{
	internal class ProxyMarshallerFactory : IProxyFactoryProvider
	{
		IProxyFactory IProxyFactoryProvider.Create(IInterceptor interceptor)
		{
			if (interceptor.IsInterface())
			{
				return new InterfaceProxyFactory(interceptor, GeneratedProxyCache.Cache);
			}

			if (interceptor.IsDelegate())
			{
				return new DelegateProxyFactory(interceptor, GeneratedProxyCache.Cache);
			}

			throw new NotImplementedException();
		}
	}
}
