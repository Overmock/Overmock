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
		/// Intercepts the specified implementation.
		/// </summary>
		/// <typeparam name="T">The type of the t interface.</typeparam>
		/// <typeparam name="TTarget">The type of the t implementation.</typeparam>
		/// <param name="implementation">The implementation.</param>
		/// <param name="memberInvoked">The member invoked.</param>
		/// <returns>TypeInterceptor&lt;TInterface&gt;.</returns>
		public static T WithCallback<T, TTarget>(TTarget implementation, InvocationAction memberInvoked)
			where T : class
			where TTarget : T
		{
			return new TargetedCallbackInterceptor<T>(implementation, memberInvoked);
		}

        /// <summary>
        /// Intercepts member calls using the provided the handlers.
        /// </summary>
        /// <typeparam name="T">The interface type to proxy.</typeparam>
        /// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
        /// <param name="target"></param>
        /// <param name="handlers">The handlers.</param>
        /// <returns>The interceptor.</returns>
        public static T WithHandlers<T, TTarget>(TTarget target, params IInvocationHandler[] handlers)
			where T : class
			where TTarget : T
		{
			return new TargetedHandlersInterceptor<T>(target, handlers);
		}

		/// <summary>
		/// Intercepts member calls using the provided the handlers.
		/// </summary>
		/// <typeparam name="T">The interface type to proxy.</typeparam>
		/// <typeparam name="TTarget">The target type to proxy to.</typeparam>
		/// <param name="target"></param>
		/// <param name="handlers">The handlers.</param>
		/// <returns>The interceptor.</returns>
		public static T WithHandlers<T, TTarget>(TTarget target, IEnumerable<IInvocationHandler> handlers)
			where T : class
			where TTarget : T
		{
			return new TargetedHandlersInterceptor<T>(target, handlers);
		}

        /// <summary>
        /// Withes the inovcation chain.
        /// </summary>
        /// <typeparam name="TInterface">The type of the t interface.</typeparam>
        /// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
        /// <param name="target"></param>
        /// <param name="builderAction">The builder action.</param>
        /// <returns>TInterface.</returns>
        public static TInterface WithChain<TInterface, TTarget>(TTarget target, Action<IInvocationChainBuilder> builderAction)
			where TInterface : class
			where TTarget : TInterface
		{
			var builder = new InvocationChainBuilder();

			builderAction(builder);

			var handler = builder.Build();

			return new TargetedSingleHandlerInterceptor<TInterface>(target, handler);
		}
	}
}
