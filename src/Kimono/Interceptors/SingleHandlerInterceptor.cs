

namespace Kimono.Interceptors
{
	/// <summary>
	/// Class SingleHandlerInterceptor.
	/// Implements the <see cref="Kimono.Interceptor{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.Interceptor{T}" />
	public class SingleHandlerInterceptor<T> : Interceptor<T> where T : class
	{
		private readonly IInvocationHandler _handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="SingleHandlerInterceptor{T}"/> class.
		/// </summary>
		/// <param name="handler">The handler.</param>
		public SingleHandlerInterceptor(IInvocationHandler handler)
		{
			_handler = handler;
		}

		/// <inheritdoc />
		protected override void MemberInvoked(IInvocationContext context)
		{
			_handler.Handle(context);
		}
	}

	/// <summary>
	/// Class SingleHandlerInterceptor.
	/// Implements the <see cref="Kimono.Interceptor{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.Interceptor{T}" />
	public class TargetedSingleHandlerInterceptor<T> : SingleHandlerInterceptor<T> where T : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TargetedSingleHandlerInterceptor{T}"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="handler">The handler.</param>
		public TargetedSingleHandlerInterceptor(T target, IInvocationHandler handler) : base(handler)
		{
			Target = target;
		}
	}
}
