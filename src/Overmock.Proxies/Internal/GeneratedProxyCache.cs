using System.Collections.Concurrent;

namespace Overmock.Proxies.Internal
{
    internal class GeneratedProxyCache : IProxyCache
	{
		public static readonly ConcurrentDictionary<Type, IProxyGenerator> _cache = new ConcurrentDictionary<Type, IProxyGenerator>();
		private static readonly IProxyCache _proxyCache = new GeneratedProxyCache();

		private GeneratedProxyCache()
		{
		}

		public static IProxyCache Cache => _proxyCache;

		public bool Contains(Type type)
		{
			return _cache.ContainsKey(type);
		}

		public IProxyGenerator? Get(Type type)
		{
			if (_cache.TryGetValue(type, out var value))
			{
				return value;
			}

			return null;
		}

		public bool TryGet(Type type, out IProxyGenerator? value)
		{
			return _cache.TryGetValue(type, out value);
		}

		public IProxyGenerator Set<T>(Type type, T value) where T : IProxyGenerator
		{
			_cache.TryAdd(type, value);

			return value;
		}

		public bool TrySet(Type type, IProxyGenerator value)
		{
			return _cache.TryAdd(type, value);
		}
	}
}
