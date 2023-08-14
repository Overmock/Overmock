using Kimono.Internal;
using Kimono.Proxies;

namespace Kimono
{
	/// <summary>
	/// Class DisposableInterceptor.
	/// Implements the <see cref="Kimono.Interceptor{T}" />
	/// Implements the <see cref="Kimono.IDisposableInterceptor" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.Interceptor{T}" />
	/// <seealso cref="Kimono.IDisposableInterceptor" />
	public abstract class DisposableInterceptor<T> : Interceptor<T>, IDisposableInterceptor where T : class, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableInterceptor{T}" /> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="disposer">The disposer.</param>
		protected DisposableInterceptor(T target, ITargetDisposer<T>? disposer = null) : base(target)
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
