﻿using System;
using System.Collections.Generic;

namespace Kimono.Proxies
{
    /// <summary>
    /// Class ProxyContext.
    /// </summary>
    public class ProxyContext
    {
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
        /// <param name="genericParameters"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>InvocationContext.</returns>
        internal InvocationContext GetInvocationContext(int methodId, IProxy proxy, Type[] genericParameters, object[] parameters)
        {
            return _overrides[methodId].GetInvocationContext(proxy.Interceptor, genericParameters, parameters);
        }
    }
}
