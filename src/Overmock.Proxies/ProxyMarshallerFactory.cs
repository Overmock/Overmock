using Overmock.Proxies.Internal;

namespace Overmock.Proxies
{
	internal class ProxyMarshallerFactory : IMarshallerFactory
	{
		IProxyFactory IMarshallerFactory.Create(IInterceptor interceptor)
		{
			if (interceptor.IsInterface())
			{
				return new InterfaceProxyFactory(interceptor);
			}

			if (interceptor.IsDelegate())
			{
				return new DelegateProxyMarshaller(interceptor);
			}

			throw new NotImplementedException();
		}
	}
}
