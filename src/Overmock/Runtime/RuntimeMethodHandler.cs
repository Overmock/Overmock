namespace Overmock.Runtime
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
			var overmock = Context.Overrides.First();

			var result = overmock.Handle(Context);

			if (result == null && Context.IsValueType())
			{
				return new RuntimeHandlerResult(Context.GetDefaultValueTypeValue());
			}

			return new RuntimeHandlerResult(result);
		}
	}
}
