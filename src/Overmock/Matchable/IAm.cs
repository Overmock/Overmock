namespace Overmocked.Matchable
{
    /// <summary>
    /// Interface IAm
    /// Extends the <see cref="IAny{T}" />
    /// Extends the <see cref="IFluentInterface" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IAny{T}" />
    /// <seealso cref="IFluentInterface" />
    public interface IAm<T> : IAny<T>, IFluentInterface
    {
    }
}
