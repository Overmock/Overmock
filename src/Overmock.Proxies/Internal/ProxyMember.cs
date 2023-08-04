using System.Reflection;

namespace Overmock.Proxies.Internal
{
    public class ProxyMember : IProxyMember
	{
		public ProxyMember(MethodInfo method) : this(method, method)
		{
		}

		public ProxyMember(MemberInfo member, MethodInfo method)
		{
			Member = member;
			Method = method;
		}

		public MemberInfo Member { get; }

		public MethodInfo Method { get; }

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