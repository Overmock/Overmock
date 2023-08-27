using System.ComponentModel;

namespace Overmocked.Mocking
{
    /// <summary>
    /// Interface IVerifiable
    /// Extends the <see cref="IFluentInterface" />
    /// </summary>
    /// <seealso cref="IFluentInterface" />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IVerifiable : IFluentInterface
    {
        ///// <summary>
        ///// Gets the type.
        ///// </summary>
        ///// <value>The type.</value>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //Type Type { get; }

        /// <summary>
        /// Verifies this instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Verify();
    }

    /// <summary>
    /// Interface IVerifiable
    /// Extends the <see cref="IVerifiable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IVerifiable" />
    public interface IVerifiable<T> : IVerifiable
    {
    }
}