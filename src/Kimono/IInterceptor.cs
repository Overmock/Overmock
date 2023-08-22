using System;
using Kimono.Proxies;

namespace Kimono
{
    /// <summary>
    /// Interface IInterceptor
    /// Extends the <see cref="Kimono.IInterceptor" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Kimono.IInterceptor" />
    public interface IInterceptor<T> : IInterceptor where T : class
    {
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        T? Target { get; }
    }

    /// <summary>
    /// Interface IInterceptor
    /// </summary>
    public interface IInterceptor : IFluentInterface
	{
		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
		string TypeName { get; }

		/// <summary>
		/// Gets the type of the target.
		/// </summary>
		/// <value>The type of the target.</value>
		Type TargetType { get; }

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <returns>System.Object.</returns>
		object? GetTarget();

		/// <summary>
		/// Members the invoked.
		/// </summary>
		/// <param name="context">The context.</param>
		void MemberInvoked(IInvocationContext context);

        /// <summary>
        /// Members the invoked.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="proxy">The proxy.</param>
        /// <param name="methodId">The method identifier.</param>
        /// <param name="parameters">The parameters.</param>
        object? MemberInvoked(ProxyContext context, IProxy proxy, int methodId, object[] parameters);
	}
}
