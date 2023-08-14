

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
}
