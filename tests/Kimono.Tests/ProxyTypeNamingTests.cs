namespace Kimono.Tests
{
    // Deliberately the same simple name as Nested.IAmbiguous below. Proxy type names were built
    // from Type.Name alone, so these two collided inside the single process-wide dynamic module
    // and the second one to be proxied threw "Duplicate type name within an assembly".
    public interface IAmbiguous
    {
        string Which();
    }

    public interface IShared<T>
    {
        T Get();
    }

    namespace Nested
    {
        public interface IAmbiguous
        {
            string Which();
        }
    }

    /// <summary>
    /// Regression tests for generated proxy type names being unique within the dynamic module.
    /// </summary>
    [TestClass]
    public class ProxyTypeNamingTests
    {
        private sealed class NullInterceptor<T> : Interceptor<T> where T : class
        {
            protected override void HandleInvocation(IInvocation invocation)
            {
            }
        }

        [TestMethod]
        public void InterfacesWithTheSameSimpleNameInDifferentNamespacesCanBothBeProxied()
        {
            var factory = ProxyFactory.Create();

            var first = factory.CreateInterfaceProxy(new NullInterceptor<IAmbiguous>());
            var second = factory.CreateInterfaceProxy(new NullInterceptor<Nested.IAmbiguous>());

            Assert.IsNotNull(first);
            Assert.IsNotNull(second);
            Assert.AreNotEqual(first.GetType(), second.GetType());
        }

        [TestMethod]
        public void DifferentClosedGenericsOfTheSameInterfaceCanBothBeProxied()
        {
            // IShared<int> and IShared<string> are both named "IShared`1".
            var factory = ProxyFactory.Create();

            var ints = factory.CreateInterfaceProxy(new NullInterceptor<IShared<int>>());
            var strings = factory.CreateInterfaceProxy(new NullInterceptor<IShared<string>>());

            Assert.IsNotNull(ints);
            Assert.IsNotNull(strings);
            Assert.AreNotEqual(ints.GetType(), strings.GetType());
        }

        [TestMethod]
        public void CreateProxyGeneratorCanBeCalledMoreThanOnceForTheSameType()
        {
            // Its own documentation says it ignores cached generators, so calling it twice has to
            // be legal - it used to throw on the duplicate type name.
            var factory = ProxyFactory.Create();

            var first = factory.CreateProxyGenerator(new NullInterceptor<IAmbiguous>());
            var second = factory.CreateProxyGenerator(new NullInterceptor<IAmbiguous>());

            Assert.IsNotNull(first);
            Assert.IsNotNull(second);
            Assert.AreNotSame(first, second);
        }
    }
}
