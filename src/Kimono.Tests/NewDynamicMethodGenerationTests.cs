using Kimono.Interceptors;
using Kimono.Tests.Proxies;

namespace Kimono.Tests
{
    [TestClass]
    public class NewDynamicMethodGenerationTests
    {
        [TestMethod]
        public void IVoidNoArgs_Test()
        {
            var called = true;
            var target = new VoidNoArgsClass();
            var overmock = new TargetedCallbackInterceptor<IVoidNoArgs>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.VoidNoArgs();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IIntNoArgs_Test()
        {
            var called = true;
            var target = new IntNoArgsClass();
            var overmock = new TargetedCallbackInterceptor<IIntNoArgs>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.IntNoArgs();

            Assert.IsTrue(called);
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void IStringNoArgs_Test()
        {
            var called = true;
            var target = new StringNoArgsClass();
            var overmock = new TargetedCallbackInterceptor<IStringNoArgs>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.StringNoArgs();

            Assert.IsTrue(called);
            Assert.AreEqual("hello, world!", result);
        }

        [TestMethod]
        public void IIntArgsString_Test()
        {
            var called = true;
            var target = new IntArgsStringClass();
            var overmock = new TargetedCallbackInterceptor<IIntArgsString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.IntArgsString("hello");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IVoidArgsStringString_Test()
        {
            var called = true;
            var target = new VoidArgsStringStringClass();
            var overmock = new TargetedCallbackInterceptor<IVoidArgsStringString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.VoidArgsStringString("hello", "world");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IStringArgsDoubleString_Test()
        {
            var called = true;
            var target = new StringArgsDoubleStringClass();
            var overmock = new TargetedCallbackInterceptor<IStringArgsDoubleString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.StringArgsDoubleString(420.69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual("420.69, active", result);
        }

        [TestMethod]
        public void IVoidArgsIntString_Test()
        {
            var called = true;
            var target = new VoidArgsIntStringClass();
            var overmock = new TargetedCallbackInterceptor<IVoidArgsIntString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.VoidArgsIntString(69, "active");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IStringArgsIntDoubleString_Test()
        {
            var called = true;
            var target = new StringArgsIntDoubleStringClass();
            var overmock = new TargetedCallbackInterceptor<IStringArgsIntDoubleString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.StringArgsIntDoubleString(20, 420.69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual("Account Id: 20 with the balance of $ with status: active", result);
        }

        [TestMethod]
        public void IVoidArgsIntDoubleString_Test()
        {
            var called = true;
            var target = new VoidArgsIntDoubleStringClass();
            var overmock = new TargetedCallbackInterceptor<IVoidArgsIntDoubleString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.VoidArgsIntDoubleString(20, 420.69, "active");

            Assert.IsTrue(called);
        }
    }
}
