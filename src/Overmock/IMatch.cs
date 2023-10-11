namespace Overmocked
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Matches(object value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMatch<T> : IMatch, IFluentInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Matches(T value);
    }
}
