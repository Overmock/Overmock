using Kimono.Interceptors;
using Kimono.Tests.Proxies;

namespace Kimono.Tests
{
    [TestClass]
    public class NewDynamicMethodGenerationTests
    {
        [TestMethod]
        public void IMethodNoArgsVoid_Test()
        {
            var called = true;
            var target = new MethodNoArgsVoidClass();
            var overmock = new TargetedCallbackInterceptor<IMethodNoArgsVoid>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.MethodNoArgsVoid();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IMethodNoArgsInt_Test()
        {
            var called = true;
            var target = new MethodNoArgsIntClass();
            var overmock = new TargetedCallbackInterceptor<IMethodNoArgsReturnsInt>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.MethodNoArgsInt();

            Assert.IsTrue(called);
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void IMethodStringArgReturnsInt_Test()
        {
            var called = true;
            var target = new MethodStringArgReturnsIntClass();
            var overmock = new TargetedCallbackInterceptor<IMethodStringArgReturnsInt>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.MethodStringArgReturnsInt("hello");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IMethodStringStringArgVoid_Test()
        {
            var called = true;
            var target = new MethodStringStringArgVoidClass();
            var overmock = new TargetedCallbackInterceptor<IMethodStringStringArgVoid>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.MethodStringStringArgVoid("hello", "world");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IMethodArgsDoubleStringReturnsString_Test()
        {
            var called = true;
            var target = new MethodArgsDoubleStringReturnsStringClass();
            var overmock = new TargetedCallbackInterceptor<IMethodArgsDoubleStringReturnsString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.MethodArgsDoubleStringReturnsString(420.69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual("420.69, active", result);
        }

        [TestMethod]
        public void IMethodArgIntStringReturnsVoid_Test()
        {
            var called = true;
            var target = new MethodArgIntStringReturnsVoidClass();
            var overmock = new TargetedCallbackInterceptor<IMethodArgIntStringReturnsVoid>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.MethodArgIntStringReturnsVoid(69, "active");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void IMethodArgsIntDoubleStringReturnsString_Test()
        {
            var called = true;
            var target = new MethodArgsIntDoubleStringReturnsStringClass();
            var overmock = new TargetedCallbackInterceptor<IMethodArgsIntDoubleStringReturnsString>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            var result = subject.MethodArgsIntDoubleStringReturnsString(20, 420.69, "active");

            Assert.IsTrue(called);
            Assert.AreEqual("Account Id: 20 with the balance of $ with status: active", result);
        }

        [TestMethod]
        public void IMethodArgIntDoubleStringReturnsVoid_Test()
        {
            var called = true;
            var target = new MethodArgIntDoubleStringReturnsVoidClass();
            var overmock = new TargetedCallbackInterceptor<IMethodArgIntDoubleStringReturnsVoid>(target, c => {
                called = true;
                c.Invoke();
            });

            var subject = overmock.Proxy;

            subject.MethodArgIntDoubleStringReturnsVoid(20, 420.69, "active");

            Assert.IsTrue(called);
        }
    }
}
