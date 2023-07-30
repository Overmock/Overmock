namespace Overmock.Proxies
{
	public class InvocationContext
	{
		public InvocationContext(RuntimeContext runtimeContext, object[] parameters)
		{
			RuntimeContext = runtimeContext;
			MemberName = runtimeContext.MemberName;
			Parameters = runtimeContext.MapParameters(parameters);
		}

		public RuntimeContext RuntimeContext { get; private set; }

		public object? ReturnValue { get; set; }

		public string MemberName { get; private set; }

		public Parameters Parameters { get; }
	}
}