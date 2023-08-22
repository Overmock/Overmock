using System;

namespace Overmock
{
	/// <summary>
	/// The base exception for all overmock exceptions.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class UnhandledMemberException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UnhandledMemberException"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public UnhandledMemberException(string name) : this(name, default)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnhandledMemberException" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public UnhandledMemberException(string name, Exception? innerException = null)
			: base($"Member '{name}' not configured to be handled.", innerException)
		{
		}
	}
}