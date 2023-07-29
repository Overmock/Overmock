using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Runtime.Proxies
{
	internal class ProxyMarshallerFactory : IMarshallerFactory
	{
		IMarshaller IMarshallerFactory.Create(IOvermock target, Action<SetupArgs>? argsProvider)
		{
			if (target.IsInterface())
			{
				return new InterfaceProxyMarshaller(target, argsProvider);
			}

			if (target.IsDelegate())
			{
				return new DelegateProxyMarshaller(target, argsProvider);
			}

			throw new NotImplementedException();
		}
	}
}
