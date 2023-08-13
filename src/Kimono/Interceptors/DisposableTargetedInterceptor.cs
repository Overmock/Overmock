using Kimono.Internal;
using Kimono.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class DisposableTargetedInterceptor<T> : Interceptor<T>, IDisposable where T : class, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableTargetedInterceptor{T}"/> class.
		/// </summary>
		public DisposableTargetedInterceptor(T target, ITargetDisposer<T>? disposer = null) : base(target)
		{
			Disposer = disposer ?? new TargetDisposer<T>(target);
		}

		/// <summary>
		/// Gets the disposer responsible for disposing of the <see cref="Interceptor{T}.Target"/>.
		/// </summary>
		/// <value>The disposer responsible for disposing of the <see cref="Interceptor{T}.Target"/>.</value>
		protected ITargetDisposer<T> Disposer { get; }

		/// <inheritdoc />
		protected override void MemberInvoked(IInvocationContext context)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			Disposer.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
