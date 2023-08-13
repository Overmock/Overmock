using Kimono.Internal;
using Kimono.Proxies;

namespace Kimono.Interceptors
{
    /// <summary>
    /// Class DisposableTargetedHandlersInterceptor.
    /// Implements the <see cref="TargetedSingleHandlerInterceptor{T}" />
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="TargetedHandlersInterceptor{T}" />
    /// <seealso cref="IDisposable" />
    public class DisposableTargetedSingleHandlerInterceptor<T> : TargetedSingleHandlerInterceptor<T>, IDisposable where T : class, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableTargetedSingleHandlerInterceptor{T}" /> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="handler"></param>
		/// <param name="disposer">The disposer.</param>
		public DisposableTargetedSingleHandlerInterceptor(T target, IInvocationHandler handler, ITargetDisposer<T>? disposer = null) : base(target, handler)
		{
			Disposer = disposer ?? new TargetDisposer<T>(target);
		}

		/// <summary>
		/// Gets the disposer responsible for disposing of the <see cref="Interceptor{T}.Target"/>.
		/// </summary>
		/// <value>The disposer responsible for disposing of the <see cref="Interceptor{T}.Target"/>.</value>
		protected ITargetDisposer<T> Disposer { get; }

		/// <inheritdoc />
		public void Dispose()
		{
			Disposer.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
