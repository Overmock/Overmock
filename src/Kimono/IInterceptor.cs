using System;
using System.ComponentModel;

namespace Kimono
{
    /// <summary>
    /// Base interface for intercepting method and property calls on proxied interfaces.
    /// </summary>
    /// <remarks>
    /// Interceptors are the core extension point in Kimono. When a method is called on a proxy,
    /// the proxy's generated IL code calls HandleInvocation, passing method metadata and arguments.
    /// The interceptor can then:
    /// - Inspect the method and arguments
    /// - Invoke a target implementation (if one exists)
    /// - Modify arguments or return values
    /// - Implement custom behavior (caching, logging, validation, etc.)
    /// - Short-circuit the call entirely
    /// </remarks>
    public interface IInterceptor : IFluentInterface
	{
        /// <summary>
        /// Handles a method or property invocation on the proxy.
        /// </summary>
        /// <remarks>
        /// This method is called by the generated proxy IL code for every method and property access.
        /// The implementation receives low-level metadata about the call and must return an appropriate value.
        ///
        /// Most users should inherit from Interceptor&lt;T&gt; or InterceptorBase instead of implementing
        /// this interface directly, as those base classes provide a higher-level API.
        /// </remarks>
        /// <param name="methodId">Unique identifier for the method being called (internal to the proxy).</param>
        /// <param name="genericParameters">Generic type arguments if the method is generic, otherwise empty.</param>
        /// <param name="parameters">The arguments passed to the method, boxed as objects.</param>
        /// <returns>The value to return from the method (must match the method's return type, or null for void).</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        object? HandleInvocation(int methodId, Type[] genericParameters, object[] parameters);
	}

    /// <summary>
    /// Generic interceptor interface that provides type-safe access to the proxy target.
    /// </summary>
    /// <typeparam name="T">The interface type being proxied.</typeparam>
    /// <remarks>
    /// This interface extends IInterceptor with type information about the proxied interface.
    /// It's used by the proxy generation system to determine whether the interceptor has a target
    /// implementation to forward calls to.
    /// </remarks>
    public interface IInterceptor<out T> : IInterceptor
    {
        /// <summary>
        /// Indicates whether this interceptor has a target implementation to forward calls to.
        /// </summary>
        /// <remarks>
        /// When true, the interceptor can invoke a target implementation of the interface.
        /// When false, the interceptor is standalone and provides all behavior itself.
        /// This affects the proxy generation strategy and invoker optimization.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool ContainsTarget { get; }
    }

    /// <summary>
    /// Interceptor interface for proxies that need to manage disposable resources.
    /// </summary>
    /// <typeparam name="T">The interface type being proxied.</typeparam>
    /// <remarks>
    /// Implement this interface when your interceptor needs cleanup logic. The proxy will call
    /// Dispose() when the proxy itself is disposed (if T implements IDisposable).
    /// </remarks>
    public interface IDisposableInterceptor<T> : IInterceptor<T>, IDisposable
    {
    }
}