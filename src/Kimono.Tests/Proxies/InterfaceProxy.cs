using Kimono.Proxies;

namespace Kimono.Tests.ProxyMembers
{
    public interface IInterface
	{
		string Name { get; }

		string DoSomething(string name);

		string MethodWithReturn(string name, object param);
	}

	public class InterfaceImpl : ProxyBase<IInterface>, IInterface
	{
        public InterfaceImpl() : base(null, null)
        {
        }

        public string Name => throw new NotImplementedException();

		public string DoSomething(string name)
		{
			const int methodId = 90001;
			return (string)HandleMethodCall(methodId, name)!;
		}

		public string MethodWithReturn(string name, object param)
		{
			const int methodId = 50004;
			return (string)HandleMethodCall(methodId, name, param)!;
		}
	}

	internal class InterfaceProxy : Interceptor<IInterface>
	{
		public InterfaceProxy(IInterface target) : base(target)
		{
		}

		protected override void MemberInvoked(IInvocationContext context)
		{
		}
	}
}
