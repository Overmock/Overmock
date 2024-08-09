using Kimono.Tests.Proxies;

namespace Kimono.Tests
{
    [TestClass]
    public class InterfaceProxyTests
    {
        private readonly IProxyFactory _factory = ProxyFactory.Create();

        public InterfaceProxyTests()
        {
            //var examples = new KimonoExamples();
            //examples.NoTargetWithCallbackInterceptorExample();
            //examples.TargetWithCallbackInterceptorExample();
            //examples.NoTargetWithHandlersInterceptorExample();
            //examples.TargetWithHandlersInterceptorExample();
            //examples.NoTargetWithInvocationChainInterceptorExample();
            //examples.TargetWithInvocationChainInterceptorExample();
        }

        [TestMethod]
        public void ProxyCallsMemberInvokedForMethod()
        {
            var proxy = new InterfaceProxy(new InterfaceImpl());

            var @interface = _factory.CreateInterfaceProxy(proxy);

            @interface.DoSomething("hello world");
        }


        [TestMethod]
        public void ProxyCallsCallback()
        {
            var called = false;

            var proxy = new TestCallbackInterceptor<IInterface>(callback: c => called = c.MemberName == "DoSomething");
            var @interface = _factory.CreateInterfaceProxy(proxy);
            
            @interface.DoSomething("hello world");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ProxyCallsCallbackWithParameters()
        {
            var called = false;

            var proxy = new TestCallbackInterceptor<IInterface>(callback: c => {
                called = c.MemberName == "MethodWithReturn";
                c.ReturnValue = c.GetParameter<string>("name");
            });

            var @interface = _factory.CreateInterfaceProxy(proxy);

            var actual = @interface.MethodWithReturn("hello world", new object());

            Assert.IsTrue(called);

            Assert.AreEqual("hello world", actual);
        }
    }
}