using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public class ThrowExceptionOverride : IOverride
	{
		internal ThrowExceptionOverride(Exception exception)
		{
			Exception = exception;
		}

		/// <summary>
		/// 
		/// </summary>
		public Exception Exception { get; }

		public object? Handle(RuntimeContext context)
		{
			throw Exception;
		}
	}
}