using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kimono.Internal
{
    /// <summary>
    /// Class GeneratedProxyCache.
    /// Implements the <see cref="IProxyGeneratorCache" />
    /// </summary>
    /// <seealso cref="IProxyGeneratorCache" />
    internal sealed class ProxyGeneratorCache : IProxyGeneratorCache
    {
        private readonly ITypeCache<IProxyGenerator> _cache = new TypeCache<IProxyGenerator>();

        /// <summary>
        /// Prevents a default instance of the <see cref="ProxyGeneratorCache"/> class from being created.
        /// </summary>
        public ProxyGeneratorCache()
        {
        }

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Nullable&lt;IProxyGenerator&gt;.</returns>
        public IProxyGenerator? GetGenerator(Type type)
        {
            return _cache.GetValueOrDefault(type);
        }

        /// <summary>
        /// Sets the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>IProxyGenerator.</returns>
        public IProxyGenerator<T> SetGenerator<T>(Type type, IProxyGenerator<T> value) where T : class
        {
            _cache.TrySet(type, value);

            return value;
        }
    }
}
