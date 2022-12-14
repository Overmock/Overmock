namespace Overmock
{
    /// <summary>
    /// The base exception for all overmock exceptions.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OvermockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OvermockException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public OvermockException(string? message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}