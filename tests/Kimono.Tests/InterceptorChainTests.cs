namespace Kimono.Tests
{
    /// <summary>
    /// Regression tests for interceptor chains built with <see cref="InterceptorBuilder"/>.
    /// <para>
    /// The chain used to be stored as a single <see cref="System.Collections.Generic.IEnumerator{T}"/>
    /// held on the interceptor, so it could only be walked once for the lifetime of the proxy. Every
    /// invocation after the first found it exhausted and silently ran no handlers at all - no error,
    /// just the return type's default. The existing suite never caught it because every builder test
    /// invoked exactly one method exactly once.
    /// </para>
    /// </summary>
    [TestClass]
    public class InterceptorChainTests
    {
        public interface IGreeter
        {
            string Greet(string name);

            void Touch();
        }

        private sealed class Greeter : IGreeter
        {
            public int Touched { get; private set; }

            public string Greet(string name) => $"hello {name}";

            public void Touch() => Touched++;
        }

        [TestMethod]
        public void ChainRunsOnEveryInvocationNotJustTheFirst()
        {
            var calls = 0;
            var factory = ProxyFactory.Create();

            var builder = new InterceptorBuilder()
                .AddCallback((next, invocation) =>
                {
                    calls++;
                    invocation.ReturnValue = "handled";
                    next(invocation);
                });

            var proxy = factory.CreateInterfaceProxy(builder.Build<IGreeter>());

            Assert.AreEqual("handled", proxy.Greet("first"));
            Assert.AreEqual("handled", proxy.Greet("second"));
            Assert.AreEqual("handled", proxy.Greet("third"));
            Assert.AreEqual(3, calls, "The chain must be walked once per invocation.");
        }

        [TestMethod]
        public void EveryHandlerInTheChainRunsInOrderOnEveryInvocation()
        {
            var order = new List<string>();
            var factory = ProxyFactory.Create();

            var builder = new InterceptorBuilder()
                .AddCallback((next, invocation) =>
                {
                    order.Add("first");
                    next(invocation);
                })
                .AddCallback((next, invocation) =>
                {
                    order.Add("second");
                    next(invocation);
                });

            var proxy = factory.CreateInterfaceProxy(builder.Build<IGreeter>());

            proxy.Touch();
            proxy.Touch();

            CollectionAssert.AreEqual(
                new[] { "first", "second", "first", "second" },
                order);
        }

        [TestMethod]
        public void AHandlerThatDoesNotCallNextStopsTheChainWithoutBreakingLaterInvocations()
        {
            var reached = 0;
            var factory = ProxyFactory.Create();

            var builder = new InterceptorBuilder()
                .AddCallback((next, invocation) => { /* deliberately does not call next */ })
                .AddCallback((next, invocation) =>
                {
                    reached++;
                    next(invocation);
                });

            var proxy = factory.CreateInterfaceProxy(builder.Build<IGreeter>());

            proxy.Touch();
            proxy.Touch();

            Assert.AreEqual(0, reached, "Short-circuiting must stop the chain...");

            // ...and must not leave the chain in a state that breaks the next invocation, which a
            // shared enumerator would have done.
            var secondBuilder = new InterceptorBuilder()
                .AddCallback((next, invocation) =>
                {
                    reached++;
                    next(invocation);
                });

            var secondProxy = factory.CreateInterfaceProxy(secondBuilder.Build<IGreeter>());
            secondProxy.Touch();
            secondProxy.Touch();

            Assert.AreEqual(2, reached);
        }

        [TestMethod]
        public void CallbackProxyFactoryOverloadRunsOnEveryInvocation()
        {
            // This overload builds its chain through InterceptorBuilder internally.
            var calls = 0;
            var factory = ProxyFactory.Create();

            var proxy = factory.CreateInterfaceProxy<IGreeter>(invocation =>
            {
                calls++;
                invocation.ReturnValue = "callback";
            });

            Assert.AreEqual("callback", proxy.Greet("a"));
            Assert.AreEqual("callback", proxy.Greet("b"));
            Assert.AreEqual(2, calls);
        }

        [TestMethod]
        public void ChainIsReentrantAcrossThreads()
        {
            var calls = 0;
            var factory = ProxyFactory.Create();

            var builder = new InterceptorBuilder()
                .AddCallback((next, invocation) =>
                {
                    Interlocked.Increment(ref calls);
                    invocation.ReturnValue = "threaded";
                    next(invocation);
                });

            var proxy = factory.CreateInterfaceProxy(builder.Build<IGreeter>());

            Parallel.For(0, 200, _ => Assert.AreEqual("threaded", proxy.Greet("x")));

            Assert.AreEqual(200, calls);
        }

        [TestMethod]
        public void TargetSurvivesCallbacksAddedAfterTarget()
        {
            // AddCallback returns the base IInterceptorBuilder, so this chain resolves to
            // IInterceptorBuilder.Build<T>() rather than ITargetedInterceptorBuilder.Build().
            // That overload used to build an untargeted interceptor, silently dropping the target.
            var target = new Greeter();
            var factory = ProxyFactory.Create();

            IInterceptorBuilder builder = new InterceptorBuilder();

            var interceptor = builder
                .Target<IGreeter>(target)
                // Reaching the target is always an explicit invocation.Invoke() by a handler; the
                // end of the chain does not fall through to it. With the target dropped this call
                // was a silent no-op and the proxy returned null.
                .AddCallback((next, invocation) =>
                {
                    invocation.Invoke();
                    next(invocation);
                })
                .Build<IGreeter>();

            var proxy = factory.CreateInterfaceProxy(interceptor);

            Assert.AreEqual("hello world", proxy.Greet("world"), "The target must still be reachable.");
        }

        [TestMethod]
        public void TargetedBuildReachesTheTargetOnEveryInvocation()
        {
            var target = new Greeter();
            var factory = ProxyFactory.Create();

            IInterceptorBuilder builder = new InterceptorBuilder();

            // Target() last, so this resolves to ITargetedInterceptorBuilder<T>.Build() rather than
            // the generic IInterceptorBuilder.Build<T>() that AddCallback's return type forces.
            var interceptor = builder
                .AddCallback((next, invocation) =>
                {
                    invocation.Invoke();
                    next(invocation);
                })
                .Target<IGreeter>(target)
                .Build();

            var proxy = factory.CreateInterfaceProxy(interceptor);

            proxy.Touch();
            proxy.Touch();
            proxy.Touch();

            Assert.AreEqual(3, target.Touched, "The chain must be walked, and the target reached, on every invocation.");
        }
    }
}
