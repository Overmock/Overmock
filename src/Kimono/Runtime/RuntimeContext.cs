﻿namespace Kimono
{
	/// <summary>
	/// The context for an overridden member.
	/// </summary>
	public struct RuntimeContext
	{
		private readonly IProxyMember _proxiedMember;
		private readonly object? _defaultReturnValue;
		private readonly List<RuntimeParameter> _parameters;
		private readonly Func<object, object[]?, object?> _invokeTargetHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeContext"/> class.
		/// </summary>
		/// <param name="Kimono"></param>
		/// <param name="target"></param>
		/// <param name="parameters">The parameters.</param>
		public RuntimeContext(IProxyMember proxyMember, IEnumerable<RuntimeParameter> parameters)
		{
			_proxiedMember = proxyMember;
			_defaultReturnValue = null;
			_parameters = new List<RuntimeParameter>();
			_parameters.AddRange(parameters);
			_invokeTargetHandler = proxyMember.CreateDelegate();
		}

		/// <summary>
		/// Gets the name of the Override.
		/// </summary>
		public string MemberName => ProxiedMember.Name;

		/// <summary>
		/// Gets the number of parameters for this Kimono.
		/// </summary>
		public int ParameterCount => _parameters.Count;

		/// <summary>
		/// 
		/// </summary>
		public IProxyMember ProxiedMember => _proxiedMember;

		internal InvocationContext CreateInvocationContext(IInterceptor interceptor, object[] parameters)
		{
			return new InvocationContext(this, interceptor,  parameters);
		}

		internal Func<object, object[]?, object?> GetTargetInvocationHandler()
		{
			return _invokeTargetHandler;
		}

		internal Parameters MapParameters(object[] parameters)
		{
			return new Parameters(_parameters.ToArray(), parameters);
		}
	}
}