using Microsoft.Extensions.Logging;

namespace Kimono.Tests.Logging
{
    /// <summary>
    /// Tests for edge cases in generic method mocking.
    /// These tests ensure that the IL generation correctly handles various complex generic scenarios.
    /// </summary>
    [TestClass]
    public class GenericMethodEdgeCasesTests
    {
        /// <summary>
        /// Tests mocking ILogger with multiple type constraints.
        /// ILogger.Log has: void Log<TState>(LogLevel, EventId, TState, Exception?, Func<TState, Exception?, string>)
        /// </summary>
        [TestMethod]
        public void ILogger_Log_WithGenericTState_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<ILogger<GenericMethodEdgeCasesTests>>();
            var logger = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            // Should not throw BadImageException
            logger.Log(LogLevel.Information, new EventId(1), "test state", null, (state, ex) => state);
        }

        /// <summary>
        /// Tests ILogger extension methods that call the generic Log method internally.
        /// </summary>
        [TestMethod]
        public void ILogger_LogError_Extension_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<ILogger<GenericMethodEdgeCasesTests>>();
            var logger = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            // LogError is an extension method that calls Log<TState> internally
            logger.LogError(new Exception("test"), "Error message");
        }

        /// <summary>
        /// Tests ILogger with various logging levels.
        /// </summary>
        [TestMethod]
        public void ILogger_AllLogLevels_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<ILogger<GenericMethodEdgeCasesTests>>();
            var logger = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            logger.LogTrace("Trace message");
            logger.LogDebug("Debug message");
            logger.LogInformation("Info message");
            logger.LogWarning("Warning message");
            logger.LogError("Error message");
            logger.LogCritical("Critical message");
        }

        /// <summary>
        /// Tests a generic method with multiple type parameters.
        /// </summary>
        [TestMethod]
        public void GenericMethod_MultipleTypeParameters_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IMultipleGenericParams>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            proxy.Method<string, int, bool>("test", 42, true);
        }

        /// <summary>
        /// Tests a generic method with class constraint.
        /// </summary>
        [TestMethod]
        public void GenericMethod_WithClassConstraint_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithConstraints>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            proxy.MethodWithClassConstraint<string>("test");
        }

        /// <summary>
        /// Tests a generic method with struct constraint.
        /// </summary>
        [TestMethod]
        public void GenericMethod_WithStructConstraint_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithConstraints>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            proxy.MethodWithStructConstraint<int>(42);
        }

        /// <summary>
        /// Tests a generic method with new() constraint.
        /// </summary>
        [TestMethod]
        public void GenericMethod_WithNewConstraint_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithConstraints>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            var result = proxy.MethodWithNewConstraint<TestClass>();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests a generic method with interface constraint.
        /// </summary>
        [TestMethod]
        public void GenericMethod_WithInterfaceConstraint_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithConstraints>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            var list = new List<string>();
            proxy.MethodWithInterfaceConstraint(list);
        }

        /// <summary>
        /// Tests a generic method with params array.
        /// </summary>
        [TestMethod]
        public void GenericMethod_WithParamsArray_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithParams>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            proxy.MethodWithParams("test", 1, "two", 3.0);
        }

        /// <summary>
        /// Tests a generic method returning generic type.
        /// </summary>
        [TestMethod]
        public void GenericMethod_ReturnsGenericType_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithReturn>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            var result = proxy.GetValue<string>("key");
            // Default return value will be null for reference types
            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests a generic method with nullable generic type.
        /// </summary>
        [TestMethod]
        public void GenericMethod_WithNullableGenericType_ShouldSucceed()
        {
            var interceptor = new TestInterceptor<IGenericWithReturn>();
            var proxy = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            var result = proxy.GetNullableValue<int>("key");
            Assert.IsNull(result);
        }

        private sealed class TestInterceptor<T> : Interceptor<T> where T : class
        {
            protected override void HandleInvocation(IInvocation invocation)
            {
                // Empty implementation - just intercept and return default
            }
        }

        public class TestClass
        {
            public TestClass() { }
        }
    }

    /// <summary>
    /// Test interface with multiple generic parameters.
    /// </summary>
    public interface IMultipleGenericParams
    {
        void Method<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
    }

    /// <summary>
    /// Test interface with various generic constraints.
    /// </summary>
    public interface IGenericWithConstraints
    {
        void MethodWithClassConstraint<T>(T value) where T : class;
        void MethodWithStructConstraint<T>(T value) where T : struct;
        T MethodWithNewConstraint<T>() where T : new();
        void MethodWithInterfaceConstraint<T>(T value) where T : IEnumerable<string>;
        void MethodWithMultipleConstraints<T>(T value) where T : class, IDisposable, new();
    }

    /// <summary>
    /// Test interface with params arrays.
    /// </summary>
    public interface IGenericWithParams
    {
        void MethodWithParams<T>(T first, params object[] rest);
        TResult MethodWithParamsAndReturn<T, TResult>(T first, params object[] rest);
    }

    /// <summary>
    /// Test interface with generic return types.
    /// </summary>
    public interface IGenericWithReturn
    {
        T GetValue<T>(string key);
        T? GetNullableValue<T>(string key) where T : struct;
        Task<T> GetValueAsync<T>(string key);
        IEnumerable<T> GetValues<T>();
    }
}
