using Overmock.Proxies.Tests.ProxyMembers;

namespace Overmock.Proxies.Tests
{
	[TestClass]
	public class InterfaceProxyTests
	{
		private IInterface _interface = new InterfaceImpl();

		[TestMethod]
		public void ProxyCallsMemberInvokedForMethod()
		{
			var proxy = new InterfaceProxy(_interface);

			Assert.AreNotEqual(_interface, proxy.Proxy);

			_interface = proxy.Proxy;

			_interface.DoSomething("hello world");
		}


		[TestMethod]
		public void ProxyCallsMethod()
		{
			var called = false;

			var proxy = Interceptor.For<IInterface>((c, ps) => called = c.MemberName == "Method");
			proxy.DoSomething("hello world");

			Assert.IsTrue(called);
		}


		[TestMethod]
		public void ProxyCallsMethodWithParameters()
		 {
			var called = false;

			var proxy = Interceptor.For<IInterface>((c, ps) => {
				called = c.MemberName == "MethodWithReturn";
				
				 return c.Parameters.Get("name");
			});

			var actual = proxy.MethodWithReturn("hello world", new object());

			Assert.IsTrue(called);

			Assert.AreEqual("hello world", actual);
		}
	}
}