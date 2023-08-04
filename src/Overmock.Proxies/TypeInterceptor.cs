
namespace Overmock.Proxies
{
	public class TypeInterceptor<TInterface> : Interceptor<TInterface> where TInterface : class
	{
		private readonly Action<InvocationContext> _memberInvoked;

		public TypeInterceptor(TInterface? target, Action<InvocationContext>? memberInvoked = null) : base(target)
		{
			_memberInvoked = memberInvoked ??= c => c.InvokeTarget();
		}

		protected override void MemberInvoked(InvocationContext context)
		{
			_memberInvoked.Invoke(context);
		}

		public static implicit operator TInterface(TypeInterceptor<TInterface> interceptor)
		{
			return interceptor.Proxy;
		}
	}
}
