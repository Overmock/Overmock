using Overmock.Proxies.Internal;

namespace Overmock.Proxies
{
	internal class ProxyMarshallerFactory : IMarshallerFactory
	{
		IMarshaller IMarshallerFactory.Create(IInterceptor interceptor)
		{
			if (interceptor.IsInterface())
			{
				return new InterfaceProxyMarshaller(interceptor);
			}

			if (interceptor.IsDelegate())
			{
				return new DelegateProxyMarshaller(interceptor);
			}

			throw new NotImplementedException();
		}
	}
}
