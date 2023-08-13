namespace Kimono.Proxies
{
    /// <summary>
    /// Interface IProxyDecorator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProxyDecorator<T> : IFluentInterface where T : class
    {
        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <value>The proxy.</value>
        IProxy<T> Proxy { get; }
    }
}