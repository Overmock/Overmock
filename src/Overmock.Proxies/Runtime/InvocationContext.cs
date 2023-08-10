using Overmock.Proxies.Runtime;
using System.Reflection;

namespace Overmock.Proxies
{
	public class InvocationContext
	{
		private readonly Func<object, object[]?, object?> _invokeTargetHandler;
		private readonly object[] _arguments;
		private readonly object _target;
		private readonly bool _isProperty;

		private object? _defaultReturnValue;

		public InvocationContext(RuntimeContext runtimeContext, IInterceptor interceptor, object[] parameters)
		{
			ParentContext = runtimeContext;
			
			_arguments = parameters;
			_target = interceptor.GetTarget();
			_invokeTargetHandler = runtimeContext.GetTargetInvocationHandler();

			Interceptor = interceptor;
			MemberName = runtimeContext.MemberName;
			Parameters = runtimeContext.MapParameters(parameters);
			ProxiedMember = runtimeContext.ProxiedMember;
			
			var member = runtimeContext.ProxiedMember.Member;
			_isProperty = member is PropertyInfo;

			Member = member;
		}

		public string MemberName { get; }

		public Parameters Parameters { get; }

		public RuntimeContext ParentContext { get; }

		public IInterceptor Interceptor { get; }

		public MethodInfo Method => ProxiedMember.Method;

		public object? ReturnValue { get; set; }

		public MemberInfo Member { get; }
		
		internal IProxyMember ProxiedMember { get; }

		public void InvokeTarget(bool setReturnValue = true)
		{
			var returnValue = _invokeTargetHandler(_target, _arguments);

			if (setReturnValue)
			{
				ReturnValue = returnValue;
			}
		}

		internal object? GetReturnTypeDefaultValue()
		{
			if (_defaultReturnValue == null)
			{
				_defaultReturnValue = DefaultReturnValueCache.GetDefaultValue(Method.ReturnType);
			}

			return _defaultReturnValue;
		}

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