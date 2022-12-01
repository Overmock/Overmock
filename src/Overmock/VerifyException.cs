namespace Overmock
{
    /// <summary>
    /// The exception that gets thrown when verifying a type.
    /// </summary>
    /// <seealso cref="Overmock.OvermockException" />
    public class VerifyException : OvermockException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyException"/> class.
        /// </summary>
        /// <param name="verifiable">The verifiable.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public VerifyException(IVerifiable verifiable, string message = "", Exception? innerException = default)
            : base(Ex.Message.General(verifiable, message, innerException), innerException)
        {
            Verifiable = verifiable;
        }

        /// <summary>
        /// Gets the verifiable that caused this exception.
        /// </summary>
        /// <value>The verifiable.</value>
        public IVerifiable Verifiable { get; }
    }
}