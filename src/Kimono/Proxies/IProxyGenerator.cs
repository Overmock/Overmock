namespace Kimono.Proxies
{
    /// <summary>
    /// Interface IProxyGenerator
    /// </summary>
    public interface IProxyGenerator : IFluentInterface
    {
        /// <summary>
        /// Generates the proxy.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>System.Object.</returns>
        object GenerateProxy(IInterceptor interceptor);
    }

    /// <summary>
    /// Interface IProxyGenerator
    /// Extends the <see cref="IProxyGenerator" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IProxyGenerator" />
    public interface IProxyGenerator<T> : IProxyGenerator where T : class
    {
        /// <summary>
        /// Generates the proxy.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>T.</returns>
        T GenerateProxy(IInterceptor<T> interceptor);
    }
}