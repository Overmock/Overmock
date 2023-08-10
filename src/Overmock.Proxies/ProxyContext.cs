using Overmock.Proxies.Runtime;
using System.Reflection;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyContext
    {
        private readonly IList<RuntimeContext> _overrides = new List<RuntimeContext>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal void Add(int methodId, RuntimeContext context)
        {
            _overrides.Add(context);
        }

		internal InvocationContext GetInvocationContext(int methodId, IProxy proxy, object[] parameters)
		{
            return _overrides[methodId].CreateInvocationContext(proxy.Interceptor, parameters);
		}
	}
}
