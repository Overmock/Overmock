using Kimono.Internal.MethodInvokers;
using Kimono.Proxies;

namespace Kimono
{
    /// <summary>
    /// The context for an overridden member.
    /// </summary>
    public sealed class RuntimeContext
	{
		private readonly IProxyMember _proxiedMember;
		private readonly List<RuntimeParameter> _parameters;
        private IMethodDelegateInvoker? _methodInvoker;

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

        internal void UseMethodInvoker(IMethodDelegateInvoker methodInvoker)
        {
            _methodInvoker = methodInvoker;
        }

        internal InvocationContext GetInvocationContext(IInterceptor interceptor, object[] parameters)
		{
            //if (_invocationContext is null)
            //{
            //    _invocationContext = new InvocationContext(this, interceptor, _parameters.ToArray(), parameters);
            //}

            return new InvocationContext(this, interceptor, _parameters.ToArray(), parameters);
		}

		internal IMethodDelegateInvoker GetMethodInvoker()
		{
            return _methodInvoker ?? new MethodInfoDelegateInvoker(ProxiedMember.Method);
		}
	}
}