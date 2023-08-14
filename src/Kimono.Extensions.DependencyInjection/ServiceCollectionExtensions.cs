using Microsoft.Extensions.DependencyInjection.Extensions;
using Kimono;
using Kimono.Interceptors;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Class ServiceCollectionExtensions.
    /// </summary>
    public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds a scoped <typeparamref name="TImplementation"/> and a scoped <see cref="CallbackInterceptor{TInterface}"/> proxy that wraps the
		/// <typeparamref name="TImplementation"/> and calls the <paramref name="memberInvoked"/> when a member is invoked on the <typeparamref name="TInterface"/>.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
		/// <param name="services">The services.</param>
		/// <param name="memberInvoked">The callback to invoke when a member on <typeparamref name="TInterface"/> is to be invoked.</param>
		/// <returns>IServiceCollection.</returns>
		public static IServiceCollection AddScopedProxy<TInterface, TImplementation>(this IServiceCollection services, InvocationAction memberInvoked)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			services.TryAddScoped<TImplementation>();
			return services.AddScoped<TInterface>(s =>
				Intercept.TargetedWithCallback<TInterface, TImplementation>(s.GetRequiredService<TImplementation>(), memberInvoked));
		}
	}
}
