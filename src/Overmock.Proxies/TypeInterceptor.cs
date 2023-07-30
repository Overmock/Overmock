
namespace Overmock.Proxies
{
	public class TypeInterceptor<TInterface> : Interceptor<TInterface> where TInterface : class
	{
		private readonly Func<InvocationContext, IInterceptor<TInterface>, object?> _memberInvoked;

		public TypeInterceptor(TInterface? target = null, Func<InvocationContext, IInterceptor<TInterface>, object?>? memberInvoked = null) : base(target)
		{
			_memberInvoked = memberInvoked ??= (c, i) => c.ReturnValue;
		}

		protected override void MemberInvoked(InvocationContext context)
		{
			context.ReturnValue = _memberInvoked.Invoke(context, this);
		}

		public static implicit operator TInterface(TypeInterceptor<TInterface> interceptor)
		{
			return interceptor.Proxy;
		}
	}
}
