namespace Kimono
{
	/// <summary>
	/// Class RuntimeHandlerResult.
	/// </summary>
	public class RuntimeHandlerResult
	{
		/// <summary>
		/// The empty
		/// </summary>
		public static RuntimeHandlerResult Empty = new RuntimeHandlerResult(new object());

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeHandlerResult"/> class.
		/// </summary>
		/// <param name="result">The result.</param>
		internal RuntimeHandlerResult(object? result)
		{
			Result = result;
		}

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>The result.</value>
		public object? Result { get; }
	}
}
