
namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class ThrowExceptionOverride.
	/// Implements the <see cref="Overmock.Mocking.IOverride" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IOverride" />
	public class ThrowExceptionOverride : IOverride
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ThrowExceptionOverride"/> class.
		/// </summary>
		/// <param name="exception">The exception.</param>
		internal ThrowExceptionOverride(Exception exception)
		{
			Exception = exception;
		}

		/// <summary>
		/// Gets the exception.
		/// </summary>
		/// <value>The exception.</value>
		public Exception Exception { get; }

		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public object? Handle(OvermockContext context)
		{
			throw Exception;
		}
	}
}