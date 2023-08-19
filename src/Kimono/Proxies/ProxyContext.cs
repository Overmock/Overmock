using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono.Proxies
{
    /// <summary>
    /// Class ProxyContext.
    /// </summary>
    public sealed class ProxyContext
    {
        /// <summary>
        /// The overrides
        /// </summary>
        private readonly IList<RuntimeContext> _overrides = new List<RuntimeContext>();

        /// <summary>
        /// Adds the specified method identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        internal void Add(RuntimeContext context)
        {
            _overrides.Add(context);
        }

        /// <summary>
        /// Gets the invocation context.
        /// </summary>
        /// <param name="methodId">The method identifier.</param>
        /// <param name="proxy">The proxy.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>InvocationContext.</returns>
        internal InvocationContext GetInvocationContext(int methodId, IProxy proxy, object[] parameters)
        {
            return _overrides[methodId].GetInvocationContext(proxy.Interceptor, parameters);
        }
    }
}
