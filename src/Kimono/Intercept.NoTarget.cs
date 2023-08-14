using Kimono.Interceptors;
using Kimono.Internal;

namespace Kimono
{
	/// <summary>
	/// Contains methods for generating <see cref="IInterceptor"/>s.
	/// </summary>
	public static partial class Intercept
	{
		/// <summary>
		/// Fors the specified member invoked.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <param name="memberInvoked">The member invoked.</param>
		/// <returns>TInterface.</returns>
		public static TInterface WithCallback<TInterface>(InvocationAction memberInvoked)
			where TInterface : class
		{
			return new CallbackInterceptor<TInterface>(memberInvoked);
		}

		/// <summary>
		/// Intercepts member calls using the provided the handlers.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to proxy.</typeparam>
		/// <param name="handlers">The handlers.</param>
		/// <returns>The interceptor.</returns>
		public static TInterface WithHandlers<TInterface>(params IInvocationHandler[] handlers)
			where TInterface : class
		{
			return new HandlersInterceptor<TInterface>(handlers);
		}

		/// <summary>
		/// Intercepts member calls using the provided the handlers.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to proxy.</typeparam>
		/// <param name="handlers">The handlers.</param>
		/// <returns>The interceptor.</returns>
		public static TInterface WithHandlers<TInterface>(IEnumerable<IInvocationHandler> handlers)
			where TInterface : class
		{
			return new HandlersInterceptor<TInterface>(handlers);
		}

		/// <summary>
		/// Withes the inovcation chain.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <param name="builderAction">The builder action.</param>
		/// <returns>TInterface.</returns>
		public static TInterface WithInovcationChain<TInterface>(Action<IInvocationChainBuilder> builderAction)
			where TInterface : class
		{
			var builder = new InvocationChainBuilder();

			builderAction(builder);

			var handler = builder.Build();

			return new HandlerInterceptor<TInterface>(handler);
		}
	}
}
