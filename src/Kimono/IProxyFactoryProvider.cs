namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyFactoryProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        IProxyFactory Provide(IInterceptor interceptor);
    }
}