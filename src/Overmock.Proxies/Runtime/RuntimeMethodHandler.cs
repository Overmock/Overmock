namespace Overmock.Proxies
{
    /// <summary>
	/// Handles a specific <see cref="RuntimeContext" />.
	/// </summary>
	public class RuntimeMethodHandler : RuntimeHandlerBase
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeMethodHandler" /> class with the given <see cref="RuntimeContext"/>. .
		/// </summary>
		/// <param name="runtimeContext"></param>
		public RuntimeMethodHandler(RuntimeContext runtimeContext) : base(runtimeContext)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters">The parameters used to call the overridden member.</param>
		/// <returns>An object that handles the result of the method call.</returns>
		/// <exception cref="OvermockException"></exception>
		protected override RuntimeHandlerResult HandleCore(params object[] parameters)
		{
			var context = Context.CreateInvocationContext(parameters);

			Context.Target.MemberInvoked(context);

			if (Context.ReturnValue == null && Context.MemberReturnsValueType())
			{
				return new RuntimeHandlerResult(Context.GetReturnTypeDefaultValue());
			}

			return new RuntimeHandlerResult(Context.ReturnValue);
		}
	}
}
