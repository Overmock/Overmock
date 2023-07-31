﻿using Overmock.Proxies.Runtime;
using System.Reflection;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyContext
    {
        private readonly IDictionary<Guid, RuntimeContext> _overrides = new Dictionary<Guid, RuntimeContext>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ProxyContext Add(Guid methodId, RuntimeContext context)
        {
            _overrides.Add(methodId, context);

            return this;
        }

        /// <summary>
        /// Gets the <see cref="RuntimeContext" /> for the given <see cref="MethodInfo" />.
        /// </summary>
        /// <param name="method">The key used to get the <see cref="RuntimeContext" />.</param>
        /// <returns>The <see cref="RuntimeContext" />.</returns>
        public IRuntimeHandler Get(MethodInfo method)
        {
            var attributeValue = method.GetCustomAttribute<OvermockAttribute>()?.Value as string;

            if (!Guid.TryParse(attributeValue, out Guid methodId) || !_overrides.ContainsKey(methodId))
            {
                return new UnregisteredMemberRuntimeHandler(method);
            }

            var overrideContext = _overrides[methodId];

            return new RuntimeMethodHandler(overrideContext);
        }
    }
}
