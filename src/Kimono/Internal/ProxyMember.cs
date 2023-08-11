using System.Reflection;

namespace Kimono.Internal
{
    public class ProxyMember : IProxyMember
	{
		private readonly Func<object, object[]?, object?> _delegate;

		public ProxyMember(MethodInfo method) : this(method, method)
		{
		}

		public ProxyMember(MemberInfo member, MethodInfo method)
		{
			_delegate = method.Invoke;

			Member = member;
			Method = method;
		}

		public MemberInfo Member { get; }

		public MethodInfo Method { get; }

		public string Name => Member.Name;

		public Func<object, object[]?, object?> CreateDelegate()
		{
			return _delegate;
		}
	}
}