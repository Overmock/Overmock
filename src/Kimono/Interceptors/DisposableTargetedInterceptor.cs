using Kimono.Internal;
using Kimono.Proxies;

namespace Kimono.Interceptors
{
    /// <summary>
    /// An <see cref="IDisposable"/> <see cref="Interceptor{T}"/> which handles disposing of the <seealso cref="Interceptor{T}.Target"/>.
    /// Implements the <see cref="Interceptor{T}" />
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Interceptor{T}" />
    /// <seealso cref="IDisposable" />
    public abstract class DisposableTargetedInterceptor<T> : DisposableInterceptor<T>, IDisposableInterceptor where T : class, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableTargetedInterceptor{T}" /> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="disposer">The disposer.</param>
		public DisposableTargetedInterceptor(T target, ITargetDisposer<T>? disposer = null) : base(target, disposer)
		{
		}
	}
}
