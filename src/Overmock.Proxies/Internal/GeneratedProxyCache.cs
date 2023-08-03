using System.Collections.Concurrent;

namespace Overmock.Proxies.Internal
{
    internal class GeneratedProxyCache : IProxyCache
	{
		public static readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();
		private static readonly IProxyCache _proxyCache = new GeneratedProxyCache();

		private GeneratedProxyCache()
		{
		}

		public static IProxyCache Cache => _proxyCache;

		public bool Contains(Type type)
		{
			return _cache.ContainsKey(type);
		}

		public object? Get(Type type)
		{
			if (_cache.TryGetValue(type, out var value))
			{
				return value;
			}

			return null;
		}

		public bool TryGet(Type type, out object? value)
		{
			return _cache.TryGetValue(type, out value);
		}

		public object Set<T>(Type type, T value)
		{
			_cache.TryAdd(type, value);

			return value;
		}

		public bool TrySet(Type type, object value)
		{
			return _cache.TryAdd(type, value);
		}
	}
}
