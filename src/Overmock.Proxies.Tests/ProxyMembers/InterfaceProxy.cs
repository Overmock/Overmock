
using System.Reflection;

namespace Overmock.Proxies.Tests.ProxyMembers
{
	public interface IInterface
	{
		string Name { get; }

		void DoSomething(string name);

		string MethodWithReturn(string name, object param);
	}

	public class InterfaceImpl : ProxyBase<IInterface>, IInterface
	{
		public string Name => throw new NotImplementedException();

		public void DoSomething(string name)
		{
			throw new NotImplementedException();
		}

		public string MethodWithReturn(string name, object param)
		{
			return (string)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod());
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
