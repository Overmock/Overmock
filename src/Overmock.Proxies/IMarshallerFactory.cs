namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMarshallerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        IProxyFactory Create(IInterceptor interceptor);
    }
}