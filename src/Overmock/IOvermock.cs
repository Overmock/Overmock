using Overmocked.Mocking;

namespace Overmocked
{
    /// <summary>
    /// An interface that represents an overmocked type.
    /// </summary>
    /// <seealso cref="IVerifiable" />
    public interface IOvermock : IVerifiable
    {
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns>System.Object.</returns>
        object GetTarget();
    }

    /// <summary>
    /// Represents a mocked type who's members can be overridden.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IVerifiable" />
    public interface IOvermock<T> : IVerifiable<T>, IOvermock where T : class
    {
        /// <summary>
        /// Gets the mocked object.
        /// </summary>
        /// <value>The mocked object.</value>
        T Target { get; }

        /// <summary>
        /// Determines if the provided obj equals this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the supplied obj equals this instance, <c>false</c> otherwise.</returns>
        bool Equals(Overmock<T>? obj);
    }
}