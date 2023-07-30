using Overmock.Proxies.Runtime;
using System.Reflection;

namespace Overmock.Proxies
{
	public class InvocationContext
	{
		private object? _defaultReturnValue;

		public InvocationContext(RuntimeContext runtimeContext, object[] parameters)
		{
			ParentContext = runtimeContext;

			Interceptor = runtimeContext.Interceptor;
			MemberName = runtimeContext.MemberName;
			Parameters = runtimeContext.MapParameters(parameters);
			ProxiedMember = runtimeContext.ProxiedMember;
		}

		public IInterceptor Interceptor { get; }

		public string MemberName { get; }

		public Parameters Parameters { get; }

		public RuntimeContext ParentContext { get; }

		public IProxyMember ProxiedMember { get; }

		public object? ReturnValue { get; set; }

		internal object? GetReturnTypeDefaultValue()
		{
			if (_defaultReturnValue == null)
			{
				_defaultReturnValue = DefaultReturnValueCache.GetDefaultValue(ParentContext.Interceptor.TargetType);
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