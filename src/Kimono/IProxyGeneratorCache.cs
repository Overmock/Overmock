using System;

namespace Kimono
{
    /// <summary>
    /// Interface IProxyCache
    /// </summary>
    public interface IProxyGeneratorCache : IFluentInterface
    {
        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Nullable&lt;IProxyGenerator&gt;.</returns>
        IProxyGenerator? GetGenerator(Type type);

        /// <summary>
        /// Sets the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>IProxyGenerator.</returns>
        IProxyGenerator<T> SetGenerator<T>(Type type, IProxyGenerator<T> value) where T : class;
    }
}