using System.Reflection;

namespace Overmock.Proxies.Internal
{
    public class ProxyMember : IProxyMember
	{
		public ProxyMember(MethodInfo method, IInterceptor parent) : this(method, method, parent)
		{
		}

		public ProxyMember(MemberInfo member, MethodInfo method, IInterceptor parent)
		{
			Member = member;
			Method = method;
			Parent = parent;
		}

		public MemberInfo Member { get; }

		public MethodInfo Method { get; }

		public IInterceptor Parent { get; }

		public string Name => Member.Name;

		public object? GetDefaultReturnValue()
		{
			return default(object?);
		}

		public MemberInfo GetMember()
		{
			return Member;
		}
	}
}