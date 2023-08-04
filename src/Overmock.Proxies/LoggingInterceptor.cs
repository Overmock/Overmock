namespace Overmock.Proxies
{
	public class LoggingInterceptor<T> : TypeInterceptor<T> where T : class
	{
		public LoggingInterceptor(T? target) : base(target, null)
		{
		}

		protected override void MemberInvoked(InvocationContext context)
		{

			context.InvokeTarget();
		}
	}
}
