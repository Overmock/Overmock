namespace Kimono
{
	/// <summary>
	/// Interface IProxy
	/// Extends the <see cref="Kimono.IProxy" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.IProxy" />
	public interface IProxy<T> : IProxy where T : class
	{
		//void RegisterCallback(Func<RuntimeContext, object[], object> memberInvoked);
	}

	/// <summary>
	/// Interface IProxy
	/// Extends the <see cref="Kimono.IProxy" />
	/// </summary>
	/// <seealso cref="Kimono.IProxy" />
	public interface IProxy
	{
		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		IInterceptor Interceptor { get; }

		/// <summary>
		/// Initializes the proxy context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <param name="context">The context.</param>
		void InitializeProxyContext(IInterceptor interceptor, ProxyContext context);

		/// <summary>
		/// Gets the type of the target.
		/// </summary>
		/// <returns>Type.</returns>
		Type GetTargetType();
		
		///// <summary>
		///// 
		///// </summary>
		///// <param name="parameters"></param>
		///// <returns></returns>
		//object? MemberInvoked(RuntimeContext context, object[] parameters);
	}
}