using System;

namespace Kimono
{
    /// <summary>
    /// Factory for creating dynamic interface proxies using IL generation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kimono is a dynamic proxy generation library that creates implementations of interfaces at runtime
    /// using System.Reflection.Emit. The generated proxies intercept all method and property calls,
    /// forwarding them to an interceptor for custom handling.
    /// </para>
    /// <para>
    /// Key features:
    /// - High-performance IL generation (faster than pure reflection)
    /// - Support for generic methods with complex constraints
    /// - Automatic handling of properties, events, and special methods
    /// - Proxy caching for improved performance
    /// - Full support for async/await patterns
    /// </para>
    /// <para>
    /// Use ProxyFactory.Create() to obtain a default instance, or construct your own with custom
    /// delegate factories and caching strategies.
    /// </para>
    /// </remarks>
    public interface IProxyFactory
    {
        /// <summary>
        /// Creates an interface proxy with a simple callback interceptor.
        /// </summary>
        /// <remarks>
        /// This is the simplest way to create a proxy. The callback will be invoked for every
        /// method and property call on the proxy. Use this for quick interception scenarios
        /// where you don't need complex interceptor logic.
        /// </remarks>
        /// <typeparam name="T">The interface type to proxy (must be an interface).</typeparam>
        /// <param name="callback">Action to invoke for each method/property call.</param>
        /// <returns>An instance of T that intercepts all calls.</returns>
        /// <exception cref="KimonoException">Thrown if T is not an interface or proxy generation fails.</exception>
        /// <example>
        /// <code>
        /// var proxy = factory.CreateInterfaceProxy&lt;IFoo&gt;(invocation =>
        /// {
        ///     Console.WriteLine($"Called: {invocation.MethodName}");
        /// });
        /// </code>
        /// </example>
        T CreateInterfaceProxy<T>(Action<IInvocation> callback) where T : class;

        /// <summary>
        /// Creates an interface proxy with a custom interceptor.
        /// </summary>
        /// <remarks>
        /// Use this method when you need full control over method interception, including
        /// the ability to invoke a target implementation, modify arguments, or short-circuit calls.
        /// The interceptor can implement complex logic like lazy initialization, caching, or logging.
        /// </remarks>
        /// <typeparam name="T">The interface type to proxy (must be an interface).</typeparam>
        /// <param name="interceptor">The interceptor that will handle all method/property calls.</param>
        /// <returns>An instance of T that intercepts all calls via the interceptor.</returns>
        /// <exception cref="KimonoException">Thrown if T is not an interface or proxy generation fails.</exception>
        /// <example>
        /// <code>
        /// var interceptor = new MyCustomInterceptor&lt;IFoo&gt;();
        /// var proxy = factory.CreateInterfaceProxy(interceptor);
        /// </code>
        /// </example>
        T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class;

        /// <summary>
        /// Creates an interface proxy using an interceptor builder for composable interception.
        /// </summary>
        /// <remarks>
        /// The InterceptorBuilder allows you to compose multiple interceptors in a chain,
        /// enabling scenarios like: logging → caching → actual call → response transformation.
        /// Each interceptor in the chain can pass control to the next one or short-circuit.
        /// </remarks>
        /// <typeparam name="T">The interface type to proxy (must be an interface).</typeparam>
        /// <param name="builder">Builder that creates a chain of interceptors.</param>
        /// <returns>An instance of T that intercepts all calls via the interceptor chain.</returns>
        /// <exception cref="KimonoException">Thrown if T is not an interface or proxy generation fails.</exception>
        /// <example>
        /// <code>
        /// var proxy = factory.CreateInterfaceProxy&lt;IFoo&gt;(
        ///     new InterceptorBuilder()
        ///         .AddCallback((next, inv) => { Log(inv); next(inv); })
        ///         .AddCallback((next, inv) => { Cache(inv); next(inv); })
        /// );
        /// </code>
        /// </example>
        T CreateInterfaceProxy<T>(IInterceptorBuilder builder) where T : class;

        /// <summary>
        /// Creates a reusable proxy generator for the specified interface type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Unlike CreateInterfaceProxy, this method returns a generator that can create multiple
        /// proxy instances with different interceptors. This is useful when you need to create
        /// many instances of the same proxy type with varying behavior.
        /// </para>
        /// <para>
        /// IMPORTANT: This method bypasses proxy caching. The generator it returns will create
        /// a new proxy type definition every time. Use CreateInterfaceProxy for cached generation.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The interface type to proxy (must be an interface).</typeparam>
        /// <param name="interceptor">The interceptor to use for initial type generation.</param>
        /// <returns>A generator that can create proxy instances.</returns>
        /// <exception cref="KimonoException">Thrown if T is not an interface or proxy generation fails.</exception>
        /// <example>
        /// <code>
        /// var generator = factory.CreateProxyGenerator(interceptor);
        /// var proxy1 = generator.GenerateProxy(interceptor1);
        /// var proxy2 = generator.GenerateProxy(interceptor2);
        /// </code>
        /// </example>
        IProxyGenerator<T> CreateProxyGenerator<T>(IInterceptor<T> interceptor) where T : class;
    }
}
