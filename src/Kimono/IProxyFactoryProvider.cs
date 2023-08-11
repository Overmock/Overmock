namespace Kimono
{
	/// <summary>
	/// Interface IProxyFactoryProvider
	/// </summary>
	public interface IProxyFactoryProvider
    {
		/// <summary>
		/// Provides the specified interceptor.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>IProxyFactory.</returns>
		IProxyFactory Provide(IInterceptor interceptor);
    }
}