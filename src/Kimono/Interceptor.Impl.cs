﻿namespace Kimono
{
	/// <summary>
	/// Class Interceptor.
	/// Implements the <see cref="Kimono.IInterceptor" />
	/// </summary>
	/// <seealso cref="Kimono.IInterceptor" />
	public abstract partial class Interceptor : IInterceptor
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
		/// Gets the target.
		/// </summary>
		/// <returns>System.Object.</returns>
		object? IInterceptor.GetTarget()
		{
			return GetTarget();
		}

		/// <summary>
		/// Members the invoked.
		/// </summary>
		/// <param name="context">The context.</param>
		void IInterceptor.MemberInvoked(IInvocationContext context) => MemberInvoked(context);

		/// <summary>
		/// Members the invoked.
		/// </summary>
		/// <param name="context">The context.</param>
		protected abstract void MemberInvoked(IInvocationContext context);

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
		private static readonly string _targetTypeName = $"{typeof(T).Name}_{Guid.NewGuid():N}";

		private readonly IProxyFactory _factory;
		private T? _target;
		private T? _proxy;

		/// <summary>
		/// Initializes a new instance of the <see cref="Interceptor{T}"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="factory">The factory.</param>
		protected Interceptor(T? target, IProxyFactory? factory = null) : base(typeof(T))
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
		public T? Target
		{
			get
			{
				return _target;
			}
			protected set
			{
				_target = value;
			}
		}

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
		string IInterceptor.TypeName => _targetTypeName;

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

			return proxyGenaerator == null
				? throw new InvalidOperationException($"Generator not created by ProxyFactory. {TargetType}")
				: proxyGenaerator.GenerateProxy(this);
		}
	}
}
