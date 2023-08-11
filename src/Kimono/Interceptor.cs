
namespace Kimono
{
	public abstract class Interceptor : IInterceptor
	{
		protected Interceptor(Type targetType)
		{
			TargetType = targetType;
			TypeName = targetType.Name;
		}

		public string TypeName { get; protected set; }

		public Type TargetType { get; protected set; }

		public static TInterface For<TInterface>(Action<InvocationContext> memberInvoked) where TInterface : class
		{
			return new TypeInterceptor<TInterface>(default, memberInvoked: memberInvoked);
		}

		public static TypeInterceptor<TInterface> Intercept<TInterface, TImplementation>(TImplementation implementation, Action<InvocationContext>? memberInvoked = null) where TInterface : class where TImplementation : TInterface
		{
			return new TypeInterceptor<TInterface>(implementation, memberInvoked);
		}

		object IInterceptor.GetTarget()
		{
			return GetTarget();
		}

		void IInterceptor.MemberInvoked(InvocationContext context) => MemberInvoked(context);

		protected abstract void MemberInvoked(InvocationContext context);

		protected abstract object GetTarget();
	}

	public abstract class Interceptor<T> : Interceptor, IInterceptor<T> where T : class
	{
		private static readonly string TargetTypeName = $"{typeof(T).Name}_{Guid.NewGuid():N}";

		private readonly IProxyFactory _factory;
		private readonly T? _target;
		private T? _proxy;

		protected Interceptor(T target, IProxyFactory? factory = null) : base(typeof(T))
		{
			_target = target;
			_factory = factory ?? ProxyFactoryProvider.Proxy(this);
		}

		public T Proxy => _proxy ??= Create();

		public T? Target => _target;

		string IInterceptor.TypeName => TargetTypeName;

		protected override object GetTarget()
		{
			return Target;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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
