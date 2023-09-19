using System.Reflection.Emit;

namespace Kimono.Proxies
{
    /// <summary>
    /// Interface IProxyBuilderContext
    /// </summary>
    public interface IProxyContextBuilder
    {
        /// <summary>
        /// Gets the interceptor.
        /// </summary>
        /// <value>The interceptor.</value>
        IInterceptor Interceptor { get; }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <value>The type builder.</value>
        TypeBuilder TypeBuilder { get; }

        /// <summary>
        /// Gets the proxy context.
        /// </summary>
        /// <value>The proxy context.</value>
        ProxyContext ProxyContext { get; }

        /// <summary>
        /// Gets the next method identifier.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetNextMethodId();
    }
}