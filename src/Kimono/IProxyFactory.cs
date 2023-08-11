namespace Kimono
{
	/// <summary>
	/// Represents a builder for proxies
	/// </summary>
	public interface IProxyFactory
    {
		/// <summary>
		/// Attempts to build the specified Kimono's represented type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>IProxyGenerator&lt;T&gt;.</returns>
		IProxyGenerator<T> Create<T>(IInterceptor<T> interceptor) where T : class;
    }
}