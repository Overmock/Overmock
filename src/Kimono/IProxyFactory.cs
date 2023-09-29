namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyFactory
    {
        /// <summary>
        /// 
        /// </summary>
        IDelegateFactory MethodFactory { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class;
    }
}
