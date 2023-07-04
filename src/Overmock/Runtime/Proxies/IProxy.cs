namespace Overmock.Runtime.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProxy<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        IOvermock Target { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void InitializeOvermockContext(OvermockContext context);
    }
}