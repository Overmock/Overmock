
namespace Overmock.Proxies
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

		public static TInterface For<TInterface>(Func<InvocationContext, IInterceptor<TInterface>, object?> memberInvoked) where TInterface : class
		{
			return new TypeInterceptor<TInterface>(memberInvoked: memberInvoked);
		}

		public static IInterceptor<TInterface> Intercept<TInterface>(TInterface? implementation = null, Func<InvocationContext, IInterceptor<TInterface>, object?>? memberInvoked = null) where TInterface : class
		{
			return new TypeInterceptor<TInterface>(implementation, memberInvoked);
		}

		void IInterceptor.MemberInvoked(InvocationContext context)
		{
			MemberInvoked(context);
		}

		protected abstract void MemberInvoked(InvocationContext context);
	}

	public abstract class Interceptor<T> : Interceptor, IInterceptor<T> where T : class
	{
		private static readonly string TargetTypeName = $"{typeof(T).Name}_{Guid.NewGuid():N}";

		private readonly IMarshaller _marshaller;
		private readonly T _target;
		private T? _proxy;

		protected Interceptor(T target, IMarshaller? marshaller = null) : base(typeof(T))
		{
			_target = target;
			_marshaller = marshaller ?? MarshallerFactory.Proxy(this);
		}

		public T Proxy => _proxy ??= Create();

		public T Target => _target;

		string IInterceptor.TypeName => TargetTypeName;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected T Create()
		{
			return _marshaller.Marshal<T>();
		}
	}
}
