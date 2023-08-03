namespace Overmock.Proxies
{
    /// <summary>
    /// Represents a builder for proxies
    /// </summary>
    public interface IProxyFactory
    {
        /// <summary>
        /// Attempts to build the specified overmock's represented type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		object Create();
    }
}