using Kimono;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Class ServiceCollectionExtensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static readonly IProxyFactory _proxyFactory = ProxyFactory.Create();
        /// <summary>
        /// Adds a scoped <typeparamref name="TImplementation"/> and a scoped <see cref="Interceptor{TInterface}"/> proxy that wraps the
        /// <typeparamref name="TImplementation"/> and calls the <paramref name="memberInvoked"/> when a member is invoked on the <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of the t interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="memberInvoked">The callback to invoke when a member on <typeparamref name="TInterface"/> is to be invoked.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddScopedProxy<TInterface, TImplementation>(this IServiceCollection services, Action<IInvocation> memberInvoked)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.TryAddScoped<TImplementation>();
            return services.AddScoped(s => _proxyFactory.CreateInterfaceProxy(
                new CallbackInterceptor<TInterface>(s.GetRequiredService<TImplementation>(), memberInvoked))
            );
        }

        private sealed class CallbackInterceptor<T> : Interceptor<T> where T : class
        {
            private readonly Action<IInvocation> _callback;

            public CallbackInterceptor(T implementation, Action<IInvocation> callback) : base(implementation)
            {
                _callback = callback;
            }

            protected override void HandleInvocation(IInvocation invocation)
            {
                _callback?.Invoke(invocation);
            }
        }
    }
}
