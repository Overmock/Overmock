using Overmock.Proxies.Runtime;
using System.Reflection;

namespace Overmock.Proxies
{
	public class InvocationContext
	{
		private object? _defaultReturnValue;

		public InvocationContext(RuntimeContext runtimeContext, IInterceptor interceptor, object[] parameters)
		{
			ParentContext = runtimeContext;

			Interceptor = interceptor;
			MemberName = runtimeContext.MemberName;
			Parameters = runtimeContext.MapParameters(parameters);
			ProxiedMember = runtimeContext.ProxiedMember;
		}

		public string MemberName { get; }

		public Parameters Parameters { get; }

		public RuntimeContext ParentContext { get; }

		public IInterceptor Interceptor { get; }

		public IProxyMember ProxiedMember { get; }

		public object? ReturnValue { get; set; }

		public void InvokeTarget()
		{
			ProxiedMember.Method.Invoke(Interceptor.GetTarget(), Parameters.ToObjectArray());
		}

		internal object? GetReturnTypeDefaultValue()
		{
			if (_defaultReturnValue == null)
			{
				_defaultReturnValue = DefaultReturnValueCache.GetDefaultValue(Interceptor.TargetType);
			}

			return _defaultReturnValue;
		}

		internal bool MemberReturnsValueType()
		{
			var member = ProxiedMember.GetMember();

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