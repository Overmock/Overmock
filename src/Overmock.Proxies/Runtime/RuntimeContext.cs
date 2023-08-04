﻿using System.Collections.Immutable;
using System.Reflection;

namespace Overmock.Proxies
{
	/// <summary>
	/// The context for an overridden member.
	/// </summary>
	public class RuntimeContext
	{
		private readonly IProxyMember _proxiedMember;
		private readonly object? _defaultReturnValue;
		private readonly List<RuntimeParameter> _parameters = new List<RuntimeParameter>();

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeContext"/> class.
		/// </summary>
		/// <param name="overmock"></param>
		/// <param name="target"></param>
		/// <param name="parameters">The parameters.</param>
		public RuntimeContext(IProxyMember proxyMember, IEnumerable<RuntimeParameter> parameters)
		{
			_proxiedMember = proxyMember;
			_parameters.AddRange(parameters);
			_defaultReturnValue = proxyMember.GetDefaultReturnValue();
		}

		/// <summary>
		/// Gets the name of the Override.
		/// </summary>
		public string MemberName => ProxiedMember.Name;

		/// <summary>
		/// Gets the number of parameters for this overmock.
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

		internal Parameters MapParameters(object[] parameters)
		{
			return new Parameters(_parameters.ToArray(), parameters);
		}
	}
}