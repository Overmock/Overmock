using Kimono.Tests.ProxyMembers;

namespace Kimono.Tests
{
    [TestClass]
    public class InterfaceProxyTests
    {
        private IInterface _interface = new InterfaceImpl();

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
            var proxy = new InterfaceProxy(_interface);

            Assert.AreNotEqual(_interface, proxy.Proxy);

            _interface = proxy.Proxy;

            _interface.DoSomething("hello world");
        }


        [TestMethod]
        public void ProxyCallsMethod()
        {
            var called = false;

            var proxy = Intercept.WithCallback<IInterface>(c => called = c.MemberName == "DoSomething");
            proxy.DoSomething("hello world");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ProxyCallsMethodWithParameters()
        {
            var called = false;

            var proxy = Intercept.WithCallback<IInterface>(c => {
                called = c.MemberName == "MethodWithReturn";
                c.ReturnValue = c.Parameters.Get("name");
            });

            var actual = proxy.MethodWithReturn("hello world", new object());

            Assert.IsTrue(called);

            Assert.AreEqual("hello world", actual);
        }
    }
}