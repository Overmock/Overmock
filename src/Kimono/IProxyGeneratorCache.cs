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
        /// <returns>System.Nullable&lt;IProxyGenerator&gt;.</returns>
        IProxyGenerator<T>? GetGenerator<T>() where T : class;

        /// <summary>
        /// Sets the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>IProxyGenerator.</returns>
        IProxyGenerator<T> SetGenerator<T>(IProxyGenerator<T> value) where T : class;
    }
}