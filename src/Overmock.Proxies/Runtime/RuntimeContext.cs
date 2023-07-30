using System.Collections.Immutable;
using System.Reflection;

namespace Overmock.Proxies
{
	/// <summary>
	/// The context for an overridden member.
	/// </summary>
	public class RuntimeContext
	{
		private readonly IProxyMember _proxyMember;
		private readonly IInterceptor _target;
		private readonly object? _defaultReturnValue;
		private readonly List<RuntimeParameter> _parameters = new List<RuntimeParameter>();

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeContext"/> class.
		/// </summary>
		/// <param name="overmock"></param>
		/// <param name="target"></param>
		/// <param name="parameters">The parameters.</param>
		public RuntimeContext(IInterceptor target, IProxyMember proxyMember, IEnumerable<RuntimeParameter> parameters)
		{
			_target = target;
			_proxyMember = proxyMember;
			_parameters.AddRange(parameters);
			_defaultReturnValue = proxyMember.GetDefaultReturnValue();
		}

		/// <summary>
		/// Gets the name of the Override.
		/// </summary>
		public string MemberName => ProxyMember.Name;

		/// <summary>
		/// Gets the number of parameters for this overmock.
		/// </summary>
		public int ParameterCount => _parameters.Count;

		/// <summary>
		/// 
		/// </summary>
		public IInterceptor Target => _target;

		/// <summary>
		/// 
		/// </summary>
		public IProxyMember ProxyMember => _proxyMember;

		/// <summary>
		/// Gets or Sets the value to return.
		/// </summary>
		public object? ReturnValue { get; set; }

		internal object? GetReturnTypeDefaultValue()
		{
			return _defaultReturnValue;
		}

		internal object? Invoke<T>(T target, object[] parameters) where T : class
		{
			return ReturnValue = ProxyMember.Method.Invoke(target, parameters);
		}

		internal InvocationContext CreateInvocationContext(object[] parameters)
		{
			return new InvocationContext(this,  parameters);
		}

		internal bool MemberReturnsValueType()
		{
			var member = ProxyMember.GetMember();

			if (member is MethodInfo method)
			{
				return method.ReturnType.IsValueType;
			}

			if (member is PropertyInfo property)
			{
				return property.PropertyType.IsValueType;
			}

			return false;
		}

		internal Parameters MapParameters(object[] parameters)
		{
			return new Parameters(_parameters.ToArray(), parameters);
		}
	}
}