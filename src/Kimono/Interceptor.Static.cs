using Kimono.Interceptors;
using Kimono.Internal;

namespace Kimono
{
	/// <summary>
	/// Class Interceptor.
	/// Implements the <see cref="Kimono.IInterceptor" />
	/// </summary>
	/// <seealso cref="Kimono.IInterceptor" />
	public abstract partial class Interceptor : IInterceptor
	{
		/// <summary>
		/// Fors the specified member invoked.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <param name="memberInvoked">The member invoked.</param>
		/// <returns>TInterface.</returns>
		public static TInterface WithCallback<TInterface>(Action<IInvocationContext> memberInvoked)
			where TInterface : class
		{
			return new CallbackInterceptor<TInterface>(memberInvoked);
		}

		/// <summary>
		/// Intercepts the specified implementation.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
		/// <param name="implementation">The implementation.</param>
		/// <param name="memberInvoked">The member invoked.</param>
		/// <returns>TypeInterceptor&lt;TInterface&gt;.</returns>
		public static TInterface TargetedWithCallback<TInterface, TImplementation>(TImplementation implementation, Action<IInvocationContext> memberInvoked)
			where TInterface : class
			where TImplementation : TInterface
		{
			return new TargetedCallbackInterceptor<TInterface>(implementation, memberInvoked);
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
		/// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
		/// <param name="target"></param>
		/// <param name="handlers">The handlers.</param>
		/// <returns>The interceptor.</returns>
		public static TInterface TargetedWithHandlers<TInterface, TTarget>(TTarget target, params IInvocationHandler[] handlers)
			where TInterface : class
			where TTarget : TInterface
		{
			return new TargetedHandlersInterceptor<TInterface>(target, handlers);
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
			var localHandlers = handlers;
			IInvocationHandler[]? interceptors = null;

			return new CallbackInterceptor<TInterface>(c => {
				// Wait to enumerate the handlers till we need them.
				interceptors ??= localHandlers.ToArray();

				for (int i = 0; i < interceptors.Length; i++)
				{
					interceptors[i].Handle(c);
				}
			});
		}
		
		/// <summary>
		/// Intercepts member calls using the provided the handlers.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to proxy.</typeparam>
		/// <typeparam name="TTarget">The target type to proxy to.</typeparam>
		/// <param name="target"></param>
		/// <param name="handlers">The handlers.</param>
		/// <returns>The interceptor.</returns>
		public static TInterface TargetedWithHandlers<TInterface, TTarget>(TTarget target, IEnumerable<IInvocationHandler> handlers)
			where TInterface : class
			where TTarget : TInterface
		{
			var localHandlers = handlers;
			IInvocationHandler[]? interceptors = null;

			return new TargetedCallbackInterceptor<TInterface>(target, c =>
			{
				// Wait to enumerate the handlers till we need them.
				interceptors ??= localHandlers.ToArray();

				for (int i = 0; i < interceptors.Length; i++)
				{
					interceptors[i].Handle(c);
				}
			});
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

			return new SingleHandlerInterceptor<TInterface>(handler);
		}

		/// <summary>
		/// Withes the inovcation chain.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
		/// <param name="target"></param>
		/// <param name="builderAction">The builder action.</param>
		/// <returns>TInterface.</returns>
		public static TInterface TargetedWithInovcationChain<TInterface, TTarget>(TTarget target, Action<IInvocationChainBuilder> builderAction)
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
