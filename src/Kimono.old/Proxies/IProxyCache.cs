using System;

namespace Kimono.Proxies
{
    /// <summary>
    /// Interface IProxyCache
    /// </summary>
    public interface IProxyCache : IFluentInterface
    {
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [contains] [the specified type]; otherwise, <c>false</c>.</returns>
        bool Contains(Type type);

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

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TryGet(Type type, out IProxyGenerator? value);

        /// <summary>
        /// Tries the set.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TrySet(Type type, IProxyGenerator value);
    }
}