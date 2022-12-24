namespace Overmock.Mocking
{
    /// <summary>
    /// Represents a builder for types
    /// </summary>
    public interface ITypeBuilder
    {
        /// <summary>
        /// Attempts to build the specified overmock's represented type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        T? BuildType<T>(IOvermock<T> target) where T : class;
    }
}