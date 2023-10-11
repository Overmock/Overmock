using Kimono.Tests.Proxies;

namespace Kimono.Tests
{
    [TestClass]
    public class StaticInterceptorTests
    {
        private readonly IProxyFactory _factory = ProxyFactory.Create();

        [TestMethod]
        public void WithCallbackInterceptorTest()
        {
            var called = false;
            var saveCalled = false;

            var interceptor = new TestCallbackInterceptor<IRepository>(callback: context => {
                called = true;
                if (context.Method.Name == "Save")
                {
                    saveCalled = true;
                }
            });

            var subject = _factory.CreateInterfaceProxy(interceptor);

            var returnedTrue = subject.Save(new Model { Id = 69 });

            Assert.IsFalse(returnedTrue);
            Assert.IsTrue(called);
            Assert.IsTrue(saveCalled);
        }

        //[TestMethod]
        public void WithHandlersInterceptorTest()
        {
            var interceptor = new TestCallbackInterceptor<IRepository>(callback: c => { });

            var subject = _factory.CreateInterfaceProxy(interceptor);

            var returnedTrue = subject.Save(new Model { Id = 52 });

            Assert.IsTrue(returnedTrue);
        }

        //[TestMethod]
        //public void WithHandlersEnumerableInterceptorTest()
        //{
        //    var list = new List<IInvocationHandler>() { new TestHandler() };
        //    var interceptor = Intercept.WithHandlers<IRepository>(list);

        //    var returnedTrue = interceptor.Save(new Model { Id = 69 });

        //    Assert.IsTrue(returnedTrue);
        //}

        [TestMethod]
        public void WithInovcationChainInterceptorTest()
        {
            var called = false;
            var saveCalled = false;

            var interceptor = _factory.CreateInterfaceProxy<IRepository>(invocation => {
                called = true;

                if (invocation.Method.Name == "Save")
                {
                    saveCalled = true;
                    invocation.ReturnValue = true;
                }
            });

            var returnedTrue = interceptor.Save(new Model { Id = 69 });

            Assert.IsTrue(returnedTrue);
            Assert.IsTrue(called);
            Assert.IsTrue(saveCalled);
        }

        [TestMethod]
        public void TargetedWithCallbackInterceptorTest()
        {
            var called = false;
            var saveCalled = false;

            var interceptor = new TestCallbackInterceptor<IRepository>(new Repository(), context => {
                called = true;
                if (context.Method.Name == "Save")
                {
                    context.Invoke();
                    saveCalled = true;
                    context.ReturnValue = true;
                }
            });

            var subject = _factory.CreateInterfaceProxy(interceptor);

            var returnedTrue = subject.Save(new Model { Id = 69 });

            Assert.IsTrue(returnedTrue);
            Assert.IsTrue(called);
            Assert.IsTrue(saveCalled);
        }

        //[TestMethod]
        //public void TargetedWithHandlersInterceptorTest()
        //{
        //    var interceptor = Intercept.WithHandlers<IRepository, Repository>(new Repository(), new TestHandler());

        //    var returnedTrue = interceptor.Save(new Model { Id = 52 });

        //    Assert.IsTrue(returnedTrue);
        //}

        //[TestMethod]
        //public void TargetedWithInovcationChainInterceptorTest()
        //{
        //    var firstCalled = false;
        //    var secondCalled = false;

        //    var interceptor = Intercept.WithChain<IRepository, Repository>(new Repository(), builder => {
        //        builder.Add((next, context) => {
        //            firstCalled = true;
        //            if (!secondCalled)
        //            {
        //                next(context);
        //            }
        //        })
        //        .Add((next, context) => {
        //            secondCalled = true;
        //            context.ReturnValue = true;
        //        });
        //    });

        //    var returnedTrue = interceptor.Save(new Model { Id = 52 });

        //    Assert.IsTrue(returnedTrue);
        //    Assert.IsTrue(firstCalled);
        //    Assert.IsTrue(secondCalled);
        //}

        //[TestMethod]
        //public void TargetedWithInovcationChainInterceptorDoesNotCallNextTest()
        //{
        //    var firstCalled = false;
        //    var secondCalled = false;

        //    var interceptor = Intercept.WithChain<IRepository, Repository>(new Repository(), builder => {
        //        builder.Add((next, context) => {
        //            firstCalled = true;
        //        })
        //        .Add((next, context) => {
        //            secondCalled = true;
        //            context.ReturnValue = true;
        //        });
        //    });

        //    var returnedTrue = interceptor.Save(new Model { Id = 52 });

        //    Assert.IsFalse(returnedTrue);
        //    Assert.IsTrue(firstCalled);
        //    Assert.IsFalse(secondCalled);
        //}

        [TestMethod]
        public void DisposableTargetedWithCallbackInterceptorTest()
        {
            var called = false;
            var saveCalled = false;
            var disposeCalled = false;

            var interceptor = new DisposableTestCallbackInterceptor<IDisposableRepository>(new DisposableRepository(), context => {
                called = true;
                if (context.Method.Name == "Save")
                {
                    saveCalled = true;
                    context.ReturnValue = true;
                }
                if (context.Method.Name == "Dispose")
                {
                    disposeCalled = true;
                }
                if (context.Method.Name == "IsDisposed")
                {
                    context.Invoke();
                }
            });

            using (var subject = _factory.CreateInterfaceProxy(interceptor))
            {
                var returnedTrue = subject.Save(new Model { Id = 69 });

                Assert.IsTrue(returnedTrue);
                Assert.IsTrue(called);
                Assert.IsTrue(saveCalled);
                Assert.IsFalse(disposeCalled);
            }

            Assert.IsFalse(disposeCalled);
        }

        [TestMethod]
        public void DisposableTargetedWithHandlersInterceptorTest()
        {
            var interceptor = new TestCallbackInterceptor<IDisposableRepository>(new DisposableRepository(), c => {
                if (c.Method.Name == "Save")
                {
                    c.ReturnValue = true;
                }
            });
            
            var subject = _factory.CreateInterfaceProxy(interceptor);

            var returnedTrue = subject.Save(new Model { Id = 69 });

            Assert.IsTrue(returnedTrue);
        }

        //[TestMethod]
        //public void DisposableTargetedWithInovcationChainInterceptorTest()
        //{
        //    var firstCalled = false;
        //    var secondCalled = false;

        //    var interceptor = Intercept.DisposableWithInovcationChain<IDisposableRepository, DisposableRepository>(new DisposableRepository(), builder => {
        //        builder.Add((next, context) => {
        //            firstCalled = true;
        //            if (!secondCalled)
        //            {
        //                next(context);
        //            }
        //        })
        //        .Add((next, context) => {
        //            secondCalled = true;
        //            context.ReturnValue = true;
        //        });
        //    });

        //    var returnedTrue = interceptor.Save(new Model { Id = 69 });

        //    Assert.IsTrue(returnedTrue);
        //    Assert.IsTrue(firstCalled);
        //    Assert.IsTrue(secondCalled);
        //}

        //[TestMethod]
        //public void DisposableTargetedWithInovcationChainInterceptorDoesNotCallNextTest()
        //{
        //    var firstCalled = false;
        //    var secondCalled = false;

        //    var interceptor = Intercept.DisposableWithInovcationChain<IDisposableRepository, DisposableRepository>(new DisposableRepository(), builder => {
        //        builder.Add((next, context) => {
        //            firstCalled = true;
        //        })
        //        .Add((next, context) => {
        //            secondCalled = true;
        //            context.ReturnValue = true;
        //        });
        //    });

        //    var returnedTrue = interceptor.Save(new Model { Id = 69 });

        //    Assert.IsFalse(returnedTrue);
        //    Assert.IsTrue(firstCalled);
        //    Assert.IsFalse(secondCalled);
        //}

        //private class TestHandler : IInvocationHandler
        //{
        //    public void Handle(IInvocationContext context)
        //    {
        //        if (context.MemberName == "Save")
        //        {
        //            context.ReturnValue = true;
        //        }
        //    }
        //}
    }
}
