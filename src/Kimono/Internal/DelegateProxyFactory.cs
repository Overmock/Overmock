using Kimono.Proxies;

namespace Kimono.Internal
{
    /// <summary>
    /// Class DelegateProxyFactory.
    /// Implements the <see cref="Proxies.ProxyFactory" />
    /// </summary>
    /// <seealso cref="Proxies.ProxyFactory" />
    internal sealed class DelegateProxyFactory : ProxyFactory
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateProxyFactory"/> class.
		/// </summary>
		/// <param name="cache">The cache.</param>
		public DelegateProxyFactory(IProxyCache cache) : base(cache)
        {
        }

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>IProxyBuilderContext.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		protected override IProxyContextBuilder CreateContext(IInterceptor interceptor)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Creates the core.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="marshallerContext">The marshaller context.</param>
		/// <returns>IProxyGenerator&lt;T&gt;.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		protected override IProxyGenerator<T> CreateCore<T>(IProxyContextBuilder marshallerContext)
        {
            throw new NotImplementedException();
        }
    }
}
