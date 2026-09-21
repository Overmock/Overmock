namespace Kimono.Tests
{
    /// <summary>
    /// Regression tests for target invocation being decided at proxy-type generation time.
    /// <para>
    /// The generated proxy type and its <see cref="MethodMetadata"/> array are cached per-T for the
    /// life of the process, but whether an interceptor has a target is per-instance state. Building
    /// the <c>IDelegateInvoker</c>s only when the *first* interceptor for T happened to carry a
    /// target meant a later targeted interceptor for the same T silently no-opped instead of
    /// calling through.
    /// </para>
    /// <para>
    /// These interfaces are deliberately not shared with any other fixture: the generator cache is
    /// process-static, so a test that proves ordering behaviour has to own its own types.
    /// </para>
    /// </summary>
    [TestClass]
    public class TargetInvokerCachingTests
    {
        public interface ICounter
        {
            int Increment(int by);

            void Record(string name);
        }

        public interface ICounterWithProperty
        {
            int Count { get; set; }
        }

        private sealed class Counter : ICounter
        {
            public int Total { get; private set; }

            public string LastName { get; private set; } = string.Empty;

            public int Increment(int by)
            {
                Total += by;
                return Total;
            }

            public void Record(string name) => LastName = name;
        }

        private sealed class CounterWithProperty : ICounterWithProperty
        {
            public int Count { get; set; }
        }

        /// <summary>
        /// Observes the invocation and then passes it straight through to the target, if there is
        /// one. With no target <see cref="IInvocation.Invoke"/> is a no-op and the proxy returns
        /// the return type's default.
        /// </summary>
        private sealed class PassThroughInterceptor<T> : Interceptor<T> where T : class
        {
            public PassThroughInterceptor(T target = null!) : base(target)
            {
            }

            public int Intercepted { get; private set; }

            protected override void HandleInvocation(IInvocation invocation)
            {
                Intercepted++;
                invocation.Invoke();
            }
        }

        [TestMethod]
        public void TargetedProxyCallsThroughAfterAnUntargetedProxyOfTheSameTypeWasCreatedFirst()
        {
            var factory = ProxyFactory.Create();

            // Order matters, and it is the whole point of the test: the untargeted proxy is what
            // populates the per-type generator cache.
            var untargeted = new PassThroughInterceptor<ICounter>();
            var untargetedProxy = factory.CreateInterfaceProxy(untargeted);

            Assert.AreEqual(0, untargetedProxy.Increment(5), "An untargeted proxy has nothing to call through to.");
            Assert.AreEqual(1, untargeted.Intercepted);

            // Same T, so this reuses the cached generator and the cached MethodMetadata[].
            var target = new Counter();
            var targeted = new PassThroughInterceptor<ICounter>(target);
            var targetedProxy = factory.CreateInterfaceProxy(targeted);

            var result = targetedProxy.Increment(7);

            Assert.AreEqual(7, result, "The targeted proxy must return the target's result.");
            Assert.AreEqual(7, target.Total, "The target must actually have been invoked.");
            Assert.AreEqual(1, targeted.Intercepted);
        }

        [TestMethod]
        public void TargetedProxyCallsThroughOnVoidMethodsAfterAnUntargetedProxyWasCreatedFirst()
        {
            var factory = ProxyFactory.Create();

            var untargetedProxy = factory.CreateInterfaceProxy(new PassThroughInterceptor<ICounter>());
            untargetedProxy.Record("ignored");

            var target = new Counter();
            var targetedProxy = factory.CreateInterfaceProxy(new PassThroughInterceptor<ICounter>(target));

            targetedProxy.Record("recorded");

            Assert.AreEqual("recorded", target.LastName);
        }

        [TestMethod]
        public void TargetedProxyCallsThroughToPropertyAccessors()
        {
            // Property accessors never had invokers built for them at all, regardless of ordering.
            var factory = ProxyFactory.Create();
            var target = new CounterWithProperty { Count = 41 };

            var proxy = factory.CreateInterfaceProxy(new PassThroughInterceptor<ICounterWithProperty>(target));

            Assert.AreEqual(41, proxy.Count, "The getter must call through to the target.");

            proxy.Count = 52;

            Assert.AreEqual(52, target.Count, "The setter must call through to the target.");
        }

        [TestMethod]
        public void CreateProxyGeneratorThrowsOnNullInterceptor()
        {
            var factory = ProxyFactory.Create();

            Assert.ThrowsException<ArgumentNullException>(
                () => factory.CreateProxyGenerator<ICounter>(null!));
        }
    }
}
