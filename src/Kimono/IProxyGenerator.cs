namespace Kimono
{
	/// <summary>
	/// Interface IProxyGenerator
	/// </summary>
	public interface IProxyGenerator : IFluentInterface
	{
		/// <summary>
		/// Generates the proxy.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>System.Object.</returns>
		object GenerateProxy(IInterceptor interceptor);
	}

	/// <summary>
	/// Interface IProxyGenerator
	/// Extends the <see cref="Kimono.IProxyGenerator" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.IProxyGenerator" />
	public interface IProxyGenerator<T> : IProxyGenerator where T : class
	{
		/// <summary>
		/// Generates the proxy.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>T.</returns>
		T GenerateProxy(IInterceptor<T> interceptor);
	}
}