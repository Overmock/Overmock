namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxy
    {
        /// <summary>
        /// 
        /// </summary>
        IInterceptor Interceptor { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProxy<T> : IProxy
    {
    }
}