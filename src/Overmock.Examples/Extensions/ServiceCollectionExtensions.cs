using Kimono;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Overmock.Examples
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddScopedProxy<TInterface, TImplementation>(this IServiceCollection services, Action<IInvocationContext> memberInvoked)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			services.TryAddScoped<TImplementation>();
			return services.AddScoped<TInterface>(s =>
				Interceptor.TargetedWithCallback<TInterface, TImplementation>(s.GetRequiredService<TImplementation>(), memberInvoked));
		}
	}
}
