namespace Overmock.Proxies
{
    /// <summary>
    /// Represents a builder for types
    /// </summary>
    public interface IMarshaller
    {
        /// <summary>
        /// Attempts to build the specified overmock's represented type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Marshal<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		object Marshal();
    }
}