namespace Overmocked.Matchable
{
    /// <summary>
    /// Interface IAny
    /// Extends the <see cref="IFluentInterface" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IFluentInterface" />
    public interface IAny<T> : IFluentInterface
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; }
    }
}
