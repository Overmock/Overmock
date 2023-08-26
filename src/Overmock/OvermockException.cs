using System;

namespace Overmock
{
    /// <summary>
    /// The base exception for all overmock exceptions.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class OvermockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledMemberException"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public OvermockException(string name) : this(name, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledMemberException" /> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public OvermockException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// The exception thrown when a member is called but has not been configured to.
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