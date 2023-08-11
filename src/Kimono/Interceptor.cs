namespace Kimono
{
	/// <summary>
	/// Class Interceptor.
	/// Implements the <see cref="Kimono.IInterceptor" />
	/// </summary>
	/// <seealso cref="Kimono.IInterceptor" />
	public abstract class Interceptor : IInterceptor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Interceptor"/> class.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		protected Interceptor(Type targetType)
		{
			TargetType = targetType;
			TypeName = targetType.Name;
		}

		/// <summary>
		/// Gets or sets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
		public string TypeName { get; protected set; }

		/// <summary>
		/// Gets or sets the type of the target.
		/// </summary>
		/// <value>The type of the target.</value>
		public Type TargetType { get; protected set; }

		/// <summary>
		/// Fors the specified member invoked.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <param name="memberInvoked">The member invoked.</param>
		/// <returns>TInterface.</returns>
		public static TInterface For<TInterface>(Action<InvocationContext> memberInvoked) where TInterface : class
		{
			return new TypeInterceptor<TInterface>(default, memberInvoked: memberInvoked);
		}

		/// <summary>
		/// Intercepts the specified implementation.
		/// </summary>
		/// <typeparam name="TInterface">The type of the t interface.</typeparam>
		/// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
		/// <param name="implementation">The implementation.</param>
		/// <param name="memberInvoked">The member invoked.</param>
		/// <returns>TypeInterceptor&lt;TInterface&gt;.</returns>
		public static TypeInterceptor<TInterface> Intercept<TInterface, TImplementation>(TImplementation implementation, Action<InvocationContext>? memberInvoked = null) where TInterface : class where TImplementation : TInterface
		{
			return new TypeInterceptor<TInterface>(implementation, memberInvoked);
		}

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <returns>System.Object.</returns>
		object IInterceptor.GetTarget()
		{
			return GetTarget();
		}

		/// <summary>
		/// Members the invoked.
		/// </summary>
		/// <param name="context">The context.</param>
		void IInterceptor.MemberInvoked(InvocationContext context) => MemberInvoked(context);

		/// <summary>
		/// Members the invoked.
		/// </summary>
		/// <param name="context">The context.</param>
		protected abstract void MemberInvoked(InvocationContext context);

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		protected abstract object? GetTarget();
	}

	/// <summary>
	/// Class Interceptor.
	/// Implements the <see cref="Kimono.Interceptor" />
	/// Implements the <see cref="Kimono.IInterceptor{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Kimono.Interceptor" />
	/// <seealso cref="Kimono.IInterceptor{T}" />
	public abstract class Interceptor<T> : Interceptor, IInterceptor<T> where T : class
	{
		/// <summary>
		/// The target type name
		/// </summary>
		private static readonly string TargetTypeName = $"{typeof(T).Name}_{Guid.NewGuid():N}";

		/// <summary>
		/// The factory
		/// </summary>
		private readonly IProxyFactory _factory;
		/// <summary>
		/// The target
		/// </summary>
		private readonly T? _target;
		/// <summary>
		/// The proxy
		/// </summary>
		private T? _proxy;

		/// <summary>
		/// Initializes a new instance of the <see cref="Interceptor{T}"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="factory">The factory.</param>
		protected Interceptor(T target, IProxyFactory? factory = null) : base(typeof(T))
		{
			_target = target;
			_factory = factory ?? ProxyFactoryProvider.Proxy(this);
		}

		/// <summary>
		/// Gets the proxy.
		/// </summary>
		/// <value>The proxy.</value>
		public T Proxy => _proxy ??= Create();

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <value>The target.</value>
		public T? Target => _target;

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
		string IInterceptor.TypeName => TargetTypeName;

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		protected override object? GetTarget()
		{
			return Target;
		}

		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns>T.</returns>
		/// <exception cref="System.InvalidOperationException">Generator not created by ProxyFactory. {TargetType}</exception>
		protected T Create()
		{
			var proxyGenaerator = _factory.Create<T>(this);

			if (proxyGenaerator == null)
			{
				throw new InvalidOperationException($"Generator not created by ProxyFactory. {TargetType}");
			}

			return proxyGenaerator.GenerateProxy(this);
		}
	}
}
