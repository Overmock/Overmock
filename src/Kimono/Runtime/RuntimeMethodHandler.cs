namespace Kimono
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
		/// <exception cref="KimonoException"></exception>
		protected override RuntimeHandlerResult HandleCore(IProxy proxy, params object[] parameters)
		{
			var context = Context.CreateInvocationContext(proxy.Interceptor, parameters);

			proxy.Interceptor.MemberInvoked(context);

			if (context.ReturnValue == null && context.MemberReturnsValueType())
			{
				return new RuntimeHandlerResult(context.GetReturnTypeDefaultValue());
			}

			return new RuntimeHandlerResult(context.ReturnValue);
		}
	}
}
