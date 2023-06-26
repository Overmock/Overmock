namespace Overmock.Runtime
{
	/// <summary>
	/// Handles a specific <see cref="OverrideContext" />.
	/// </summary>
	public class OverrideHandler : IOverrideHandler
	{
		private readonly OverrideContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="OverrideHandler" /> class with the given <see cref="OverrideContext"/>. .
		/// </summary>
		/// <param name="context">The context for which this handler handles. </param>
		public OverrideHandler(OverrideContext context)
		{
			_context = context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters">The parameters used to call the overridden member.</param>
		/// <returns>An IDisposable object that handles the result of the method call.</returns>
		/// <exception cref="OvermockException"></exception>
		public OverrideHandlerResult Handle(params object[] parameters)
		{
			var exception = _context.Overrides.First();

			if (exception.Exception != null)
			{
				throw exception.Exception;
			}

			if (_context.ParameterCount != parameters.Length)
			{
				throw new OvermockException(Ex.Message.NumberOfParameterMismatch);
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				_context.SetParameterValue(i, parameters[i]);
			}


			// TODO: run through all the options and call what's needed.
			throw new OvermockException();
		}
	}
}
