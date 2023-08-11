namespace Kimono
{
	/// <summary>
	/// The base exception for all Kimono exceptions.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class KimonoException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public KimonoException() : base()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KimonoException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public KimonoException(string? message, Exception? innerException = null) : base(message, innerException)
		{
		}
	}
}