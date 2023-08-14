using Kimono.Proxies;

namespace Kimono
{
    /// <summary>
    /// The context for an overridden member.
    /// </summary>
    public struct RuntimeContext
	{
		/// <summary>
		/// The proxied member
		/// </summary>
		private readonly IProxyMember _proxiedMember;
		/// <summary>
		/// The parameters
		/// </summary>
		private readonly List<RuntimeParameter> _parameters;
		/// <summary>
		/// The invoke target handler
		/// </summary>
		private readonly Func<object, object[]?, object?> _invokeTargetHandler;

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
			_invokeTargetHandler = proxyMember.CreateDelegate();
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

		/// <summary>
		/// Creates the invocation context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>InvocationContext.</returns>
		internal InvocationContext CreateInvocationContext(IInterceptor interceptor, object[] parameters)
		{
			return new InvocationContext(this, interceptor,  parameters);
		}

		/// <summary>
		/// Gets the target invocation handler.
		/// </summary>
		/// <returns>Func&lt;System.Object, System.Nullable&lt;System.Object&gt;[], System.Nullable&lt;System.Object&gt;&gt;.</returns>
		internal Func<object, object[]?, object?> GetTargetInvocationHandler()
		{
			return _invokeTargetHandler;
		}

		/// <summary>
		/// Maps the parameters.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns>Parameters.</returns>
		internal Parameters MapParameters(object[] parameters)
		{
			return new Parameters(_parameters.ToArray(), parameters);
		}
	}
}