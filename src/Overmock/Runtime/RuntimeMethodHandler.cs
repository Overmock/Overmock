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
		/// <returns>An IDisposable object that handles the result of the method call.</returns>
		/// <exception cref="OvermockException"></exception>
		protected override RuntimeHandlerResult HandleCore(params object[] parameters)
		{
			var overmock = Context.Overrides.First();

			if (overmock.Exception != null)
			{
				throw overmock.Exception;
			}

			if (overmock.ReturnProvider != null)
			{
				return new RuntimeHandlerResult(overmock.ReturnProvider.Invoke());
			}

			if (Context.ParameterCount != parameters.Length)
			{
				throw new OvermockException(Ex.Message.NumberOfParameterMismatch);
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				Context.SetParameterValue(i, parameters[i]);
			}

			if (overmock is MethodOverride methodOverride && methodOverride.Overmock != null)
			{
				var invokeResult = methodOverride.Overmock.DynamicInvoke(Context);

				return new RuntimeHandlerResult(invokeResult);
			}

			throw new NotImplementedException("This needs more work...I guess?");
		}
	}
}
