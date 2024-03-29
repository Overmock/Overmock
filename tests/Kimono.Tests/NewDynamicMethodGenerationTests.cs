﻿using Kimono.Tests.Proxies;

namespace Kimono.Tests
{
    [TestClass]
    public class NewDynamicMethodGenerationTests
    {
        private readonly IProxyFactory _factory = ProxyFactory.Create();

        [TestMethod]
        public void IVoidNoArgs_Test()
        {
            var called = true;
            var target = new VoidNoArgsClass();
            var overmock = new TestCallbackInterceptor<IVoidNoArgs>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IIntNoArgs>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IStringNoArgs>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IIntArgsString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IVoidArgsStringString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<ILongArgsIntString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IStringArgsDoubleString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IVoidArgsIntString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IStringArgsIntDoubleString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IVoidArgsIntDoubleString>(target, c => {
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
            var overmock = new TestCallbackInterceptor<IModelArgsInt>(target, c =>
            {
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
            var overmock = new TestCallbackInterceptor<ITGenTArgsInt>(target, c => {
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
            var overmock = new TestCallbackInterceptor<ITGenTArgsTInt>(target, c => {
                called = true;
                c.Invoke();
                c.ReturnValue = new Model();
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            var model = subject.TGenTArgsTInt(new Model(), 52);

            Assert.IsTrue(called);
            Assert.IsNotNull(model);
        }
    }
}
