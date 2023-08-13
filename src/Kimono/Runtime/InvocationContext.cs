using Kimono.Proxies;
using Kimono.Runtime;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// Class InvocationContext.
    /// </summary>
    public class InvocationContext : IInvocationContext
	{
		private readonly Func<object, object[]?, object?> _invokeTargetHandler;
		private readonly object[] _arguments;
		private readonly object? _target;

		private object? _defaultReturnValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="InvocationContext"/> class.
		/// </summary>
		/// <param name="runtimeContext">The runtime context.</param>
		/// <param name="interceptor">The interceptor.</param>
		/// <param name="parameters">The parameters.</param>
		public InvocationContext(RuntimeContext runtimeContext, IInterceptor interceptor, object[] parameters)
		{
			_arguments = parameters;
			_target = interceptor.GetTarget();
			_invokeTargetHandler = runtimeContext.GetTargetInvocationHandler();

			Interceptor = interceptor;
			MemberName = runtimeContext.MemberName;
			Parameters = runtimeContext.MapParameters(parameters);
			ProxiedMember = runtimeContext.ProxiedMember;
			
			var member = runtimeContext.ProxiedMember.Member;

			Member = member;
		}

		/// <summary>
		/// Gets the name of the member.
		/// </summary>
		/// <value>The name of the member.</value>
		public string MemberName { get; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public Parameters Parameters { get; }

		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		public IInterceptor Interceptor { get; }

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		public MethodInfo Method => ProxiedMember.Method;

		/// <summary>
		/// Gets or sets the return value.
		/// </summary>
		/// <value>The return value.</value>
		public object? ReturnValue { get; set; }

		/// <summary>
		/// Gets the member.
		/// </summary>
		/// <value>The member.</value>
		public MemberInfo Member { get; }

		/// <summary>
		/// Gets the proxied member.
		/// </summary>
		/// <value>The proxied member.</value>
		internal IProxyMember ProxiedMember { get; }

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <returns>T.</returns>
		public T GetParameter<T>(string name)
		{
			return Parameters.Get<T>(name);
		}

		/// <summary>
		/// Invokes the target.
		/// </summary>
		/// <param name="setReturnValue">if set to <c>true</c> [set return value].</param>
		public void InvokeTarget(bool setReturnValue = true)
		{
			if (_target is not null)
			{
				var returnValue = _invokeTargetHandler(_target, _arguments);

				if (setReturnValue)
				{
					ReturnValue = returnValue;
				}
			}
		}

		/// <summary>
		/// Gets the return type default value.
		/// </summary>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		internal object? GetReturnTypeDefaultValue()
		{
			return _defaultReturnValue ??= DefaultReturnValueCache.GetDefaultValue(Method.ReturnType);
		}

		/// <summary>
		/// Members the type of the returns value.
		/// </summary>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		internal bool MemberReturnsValueType()
		{
			var member = ProxiedMember.Member;

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
	}
}