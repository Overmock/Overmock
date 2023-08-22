using System;
using System.Collections.Concurrent;
using Kimono.Proxies;

namespace Kimono.Internal
{
    /// <summary>
    /// Class GeneratedProxyCache.
    /// Implements the <see cref="Proxies.IProxyCache" />
    /// </summary>
    /// <seealso cref="Proxies.IProxyCache" />
    internal sealed class GeneratedProxyCache : IProxyCache
	{
		/// <summary>
		/// The cache
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "")]
		public static readonly ConcurrentDictionary<Type, IProxyGenerator> _cache = new ConcurrentDictionary<Type, IProxyGenerator>();

		/// <summary>
		/// The proxy cache
		/// </summary>
		private static readonly IProxyCache _proxyCache = new GeneratedProxyCache();

		/// <summary>
		/// Prevents a default instance of the <see cref="GeneratedProxyCache"/> class from being created.
		/// </summary>
		private GeneratedProxyCache()
		{
		}

		/// <summary>
		/// Gets the cache.
		/// </summary>
		/// <value>The cache.</value>
		public static IProxyCache Cache => _proxyCache;

		/// <summary>
		/// Determines whether this instance contains the object.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [contains] [the specified type]; otherwise, <c>false</c>.</returns>
		public bool Contains(Type type)
		{
			return _cache.ContainsKey(type);
		}

		/// <summary>
		/// Gets the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Nullable&lt;IProxyGenerator&gt;.</returns>
		public IProxyGenerator? GetGenerator(Type type)
		{
			if (_cache.TryGetValue(type, out var value))
			{
				return value;
			}

			return null;
		}

		/// <summary>
		/// Tries the get.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool TryGet(Type type, out IProxyGenerator? value)
		{
			return _cache.TryGetValue(type, out value);
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
			_cache.TryAdd(type, value);

			return value;
		}

		/// <summary>
		/// Tries the set.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool TrySet(Type type, IProxyGenerator value)
		{
			return _cache.TryAdd(type, value);
		}
	}
}
