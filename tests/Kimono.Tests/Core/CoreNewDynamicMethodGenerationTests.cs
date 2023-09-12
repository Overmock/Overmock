using Kimono.Core;
using Kimono.Tests.Proxies;

namespace Kimono.Tests.Core
{
    [TestClass]
    public class CoreNewDynamicMethodGenerationTests
    {
        private readonly IProxyFactory _factory = ProxyFactory.Create();
        [TestMethod]
        public void IVoidNoArgs_Test()
        {
            var called = true;
            var target = new VoidNoArgsClass();
            var overmock = new TestInterceptor<IVoidNoArgs>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.VoidNoArgs();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IIntNoArgs_Test()
        {
            var called = true;
            var target = new IntNoArgsClass();
            var overmock = new TestInterceptor<IIntNoArgs>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var result = subject.IntNoArgs();

            Assert.IsTrue(called);
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void IStringNoArgs_Test()
        {
            var called = true;
            var target = new StringNoArgsClass();
            var overmock = new TestInterceptor<IStringNoArgs>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var result = subject.StringNoArgs();

            Assert.IsTrue(called);
            Assert.AreEqual("hello, world!", result);
        }

        [TestMethod]
        public void IIntArgsString_Test()
        {
            var called = true;
            var target = new IntArgsStringClass();
            var overmock = new TestInterceptor<IIntArgsString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.IntArgsString("hello");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IVoidArgsStringString_Test()
        {
            var called = true;
            var target = new VoidArgsStringStringClass();
            var overmock = new TestInterceptor<IVoidArgsStringString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.VoidArgsStringString("hello", "world");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ILongArgsIntString_Test()
        {
            var called = true;
            var target = new LongArgsIntStringClass();
            var overmock = new TestInterceptor<ILongArgsIntString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var result = subject.LongArgsIntString(69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual(420, result);
        }

        [TestMethod]
        public void IStringArgsDoubleString_Test()
        {
            var called = true;
            var target = new StringArgsDoubleStringClass();
            var overmock = new TestInterceptor<IStringArgsDoubleString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var result = subject.StringArgsDoubleString(420.69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual("420.69, active", result);
        }

        [TestMethod]
        public void IVoidArgsIntString_Test()
        {
            var called = true;
            var target = new VoidArgsIntStringClass();
            var overmock = new TestInterceptor<IVoidArgsIntString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.VoidArgsIntString(69, "active");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IStringArgsIntDoubleString_Test()
        {
            var called = true;
            var target = new StringArgsIntDoubleStringClass();
            var overmock = new TestInterceptor<IStringArgsIntDoubleString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var result = subject.StringArgsIntDoubleString(20, 420.69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual("Account Id: 20 with the balance of $ with status: active", result);
        }

        [TestMethod]
        public void IVoidArgsIntDoubleString_Test()
        {
            var called = true;
            var target = new VoidArgsIntDoubleStringClass();
            var overmock = new TestInterceptor<IVoidArgsIntDoubleString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.VoidArgsIntDoubleString(20, 420.69, "active");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IModelArgsInt_Test()
        {
            var called = false;
            var target = new ModelArgsIntClass();
            var overmock = new TestInterceptor<IModelArgsInt>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var model = subject.ModelArgsInt(52);

            Assert.IsTrue(called);
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void ITGArgsTArgsInt_Test()
        {
            var called = false;
            var target = new ITGenTArgsIntClass();
            var overmock = new TestInterceptor<ITGenTArgsInt>(target, c => {
                called = true;
                c.Invoke();
                c.ReturnValue = new Model();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var model = subject.TGenTArgsInt<Model>(52);

            Assert.IsTrue(called);
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void ITGArgsTArgsTInt_Test()
        {
            var called = false;
            var target = new ITGenTArgsTIntClass();
            var overmock = new TestInterceptor<ITGenTArgsTInt>(target, c => {
                called = true;
                c.Invoke();
                c.ReturnValue = new Model();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var model = subject.TGenTArgsTInt(new Model(), 52);

            Assert.IsTrue(called);
            Assert.IsNotNull(model);
        }

        private sealed class TestInterceptor<T> : Kimono.Core.Interceptor<T> where T : class
        {
            private readonly Action<IInvocation> _callback;

            public TestInterceptor(T target, Action<IInvocation> callback) : base(target)
            {
                _callback = callback;
            }

            protected override void HandleInvocation(IInvocation invocation)
            {
                _callback.Invoke(invocation);
            }
        }
    }
}
