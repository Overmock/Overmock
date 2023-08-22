using System;
using System.Collections.Generic;
using Kimono.Interceptors.Internal;
using Kimono.Internal;
using Kimono.Proxies;

namespace Kimono.Interceptors
{
    /// <summary>
    /// Class DisposableTargetedHandlersInterceptor.
    /// Implements the <see cref="Kimono.Interceptors.TargetedHandlersInterceptor{T}" />
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Kimono.Interceptors.TargetedHandlersInterceptor{T}" />
    /// <seealso cref="IDisposable" />
    public sealed class DisposableTargetedHandlersInterceptor<T> : TargetedHandlersInterceptor<T>, IDisposableInterceptor where T : class, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableTargetedHandlersInterceptor{T}" /> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="handlers">The handlers.</param>
		/// <param name="disposer">The disposer.</param>
		public DisposableTargetedHandlersInterceptor(T target, IEnumerable<IInvocationHandler> handlers, ITargetDisposer<T>? disposer = null) : base(target, handlers)
		{
			Disposer = disposer ?? new TargetDisposer<T>(target);
		}

		/// <summary>
		/// Gets the disposer responsible for disposing of the <see cref="Interceptor{T}.Target"/>.
		/// </summary>
		/// <value>The disposer responsible for disposing of the <see cref="Interceptor{T}.Target"/>.</value>
		private ITargetDisposer<T> Disposer { get; }

		/// <inheritdoc />
		public void Dispose()
		{
			Disposer.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
