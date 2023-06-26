namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public class ThrowExceptionOverride : IOverride
	{
		internal ThrowExceptionOverride(Exception? exception = default)
		{
			Exception = exception;
		}

		/// <summary>
		/// 
		/// </summary>
		public Exception? Exception { get; }
	}
}