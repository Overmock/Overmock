using System;
using System.Threading;
using Kimono.Internal;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProxyCache
    {
        private static IProxyGeneratorCache _current = new ProxyGeneratorCache();

        /// <summary>
        /// Gets the current cache.
        /// </summary>
        /// <value>The cache.</value>
        public static IProxyGeneratorCache Current => _current;

        /// <summary>
        /// Specifies the cache to use.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IProxyGeneratorCache Use(IProxyGeneratorCache instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return Interlocked.Exchange(ref _current, instance);
        }
    }
}