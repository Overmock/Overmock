namespace Kimono.Proxies
{
	/// <summary>
	/// Class ProxyBase.
	/// Implements the <see cref="IProxy" />
	/// </summary>
	/// <seealso cref="IProxy" />
	public abstract class ProxyBase : IProxy
	{
		private ProxyContext? _proxyContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyBase{T}"/> class.
		/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		protected ProxyBase()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
		}

		/// <summary>
		/// The <see cref="Interceptor.GetTarget" />s <see cref="Type" />.
		/// </summary>
		/// <value>The type of the target.</value>
		public Type TargetType { get; protected set; }

		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		public IInterceptor Interceptor { get; private set; }

		/// <summary>
		/// Handles the method call.
		/// </summary>
		/// <param name="methodId">The method identifier.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		protected object? HandleMethodCall(int methodId, params object[] parameters)
		{
			var invocationContext = _proxyContext.GetInvocationContext(methodId, this, parameters);

			Interceptor.MemberInvoked(invocationContext);

			if (invocationContext.ReturnValue == null && invocationContext.MemberReturnsValueType())
			{
				return invocationContext.GetReturnTypeDefaultValue();
			}

			return invocationContext.ReturnValue;
		}

		/// <summary>
		/// Initializes the proxy context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <param name="context">The context.</param>
		void IProxy.InitializeProxyContext(IInterceptor interceptor, ProxyContext context)
		{
			Interceptor = interceptor;
			_proxyContext = context;
		}

		/// <summary>
		/// Gets the type of the target.
		/// </summary>
		/// <returns>Type.</returns>
		Type IProxy.GetTargetType()
		{
			return TargetType;
		}
	}

	/// <inheritdoc />
	public abstract class ProxyBase<T> : ProxyBase, IProxy<T> where T : class
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyBase{T}"/> class.
		/// </summary>
		public ProxyBase()
		{
			TargetType = typeof(T);
		}
    }
}