using Kimono.Proxies;
using Kimono.Proxies.Internal.MethodInvokers;
using System;
using System.Collections.Generic;

namespace Kimono
{
    /// <summary>
    /// The context for an overridden member.
    /// </summary>
    public sealed class RuntimeContext
    {
        private readonly IProxyMember _proxiedMember;
        private readonly List<RuntimeParameter> _parameters;
        private IDelegateInvoker? _methodInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeContext" /> class.
        /// </summary>
        /// <param name="proxyMember">The proxy member.</param>
        /// <param name="parameters">The parameters.</param>
        public RuntimeContext(IProxyMember proxyMember, IEnumerable<RuntimeParameter> parameters)
        {
            _proxiedMember = proxyMember;
            _parameters = new List<RuntimeParameter>();
            _parameters.AddRange(parameters);
        }

        /// <summary>
        /// Gets the name of the Override.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName => ProxiedMember.Name;

        /// <summary>
        /// Gets the number of parameters for this Kimono.
        /// </summary>
        /// <value>The parameter count.</value>
        public int ParameterCount => _parameters.Count;

        /// <summary>
        /// Gets the proxied member.
        /// </summary>
        /// <value>The proxied member.</value>
        public IProxyMember ProxiedMember => _proxiedMember;

        internal IReadOnlyList<RuntimeParameter> GetParameters()
        {
            return _parameters;
        }

        internal void UseMethodInvoker(IDelegateInvoker methodInvoker)
        {
            _methodInvoker = methodInvoker;
        }

        internal InvocationContext GetInvocationContext(IInterceptor interceptor, Type[] genericParameters, object[] parameters)
        {
            return new InvocationContext(this, interceptor, genericParameters, _parameters.ToArray(), parameters);
        }

        internal IDelegateInvoker GetMethodInvoker()
        {
            return _methodInvoker ?? new MethodInfoDelegateInvoker(ProxiedMember.Method);
        }
    }
}