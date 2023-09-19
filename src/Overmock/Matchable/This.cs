using System;

namespace Overmocked.Matchable
{
    /// <summary>
    /// Class This.
    /// Implements the <see cref="Any{T}" />
    /// Implements the <see cref="IAm{T}" />
    /// Implements the <see cref="IEquatable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Any{T}" />
    /// <seealso cref="IAm{T}" />
    /// <seealso cref="IEquatable{T}" />
    public class This<T> : MatchExpression<T>, IAm<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="This{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        internal This(T value) : base(p => value.Equals(p))
        {
        }
    }
}
