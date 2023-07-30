
namespace Overmock.Proxies.Tests.ProxyMembers
{
	public interface IInterface
	{
		string Name { get; }

		void DoSomething(string name);

		string MethodWithReturn(string name);
	}

	public class InterfaceImpl : IInterface
	{
		public string Name => throw new NotImplementedException();

		public void DoSomething(string name)
		{
			throw new NotImplementedException();
		}

		public string MethodWithReturn(string name)
		{
			throw new NotImplementedException();
		}
	}

	internal class InterfaceProxy : Interceptor<IInterface>
	{
		public InterfaceProxy(IInterface target) : base(target)
		{
		}

		protected override void MemberInvoked(InvocationContext context)
		{

		}
	}
}
