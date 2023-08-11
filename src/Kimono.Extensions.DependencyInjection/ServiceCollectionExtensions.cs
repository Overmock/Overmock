﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Kimono;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddScopedProxy<TInterface, TImplementation>(this IServiceCollection services, Action<InvocationContext> memberInvoked)
			where TInterface : class
			where TImplementation : class, TInterface
		{
			services.TryAddScoped<TImplementation>();
			return services.AddScoped<TInterface>(s =>
				Interceptor.Intercept<TInterface, TImplementation>(s.GetRequiredService<TImplementation>(), memberInvoked));
		}
	}
}