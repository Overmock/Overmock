using Kimono;
using Kimono.Tests.Examples;
using Kimono.Tests.ProxyMembers;

namespace Kimono.Tests
{
	[TestClass]
	public class InterfaceProxyTests
	{
		private IInterface _interface = new InterfaceImpl();

		public InterfaceProxyTests()
		{
			// TODO: Turn the below in to unit tests.
			//KimonoExamples examples = new KimonoExamples();
			//examples.NoTargetWithHandlersInterceptorExample();
			//examples.TargetWithHandlersInterceptorExample();
			//examples.NoTargetWithCallbackInterceptorExample();
			//examples.TargetWithHandlersInterceptorExample();
			//examples.TargetWithInvocationChainInterceptorExample();
			//examples.NoTargetWithInvocationChainInterceptorExample();
		}

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

			var proxy = Interceptor.WithCallback<IInterface>(c => called = c.MemberName == "DoSomething");
			proxy.DoSomething("hello world");

			Assert.IsTrue(called);
		}
		
		[TestMethod]
		public void ProxyCallsMethodWithParameters()
		{
			var called = false;

			var proxy = Interceptor.WithCallback<IInterface>(c => {
				called = c.MemberName == "MethodWithReturn";
				c.ReturnValue = c.Parameters.Get("name");
			});

			var actual = proxy.MethodWithReturn("hello world", new object());

			Assert.IsTrue(called);

			Assert.AreEqual("hello world", actual);
		}
	}
}