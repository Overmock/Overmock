using System;
using System.Linq.Expressions;

namespace Overmocked.Matchable
{
    /// <summary>
    /// Class Any.
    /// Implements the <see cref="ParameterValue{T}" />
    /// Implements the <see cref="IAny{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ParameterValue{T}" />
    /// <seealso cref="IAny{T}" />
    public class Any<T> : MatchExpression<T>, IAny<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public Any() : base(p => p is T)
        {
        }

        /// <inheritdoc />
        public override bool Equals(T other)
        {
            return true;
        }

        /// <inheritdoc />
        public bool Equals(Any<T>? other)
        {
            return true;
        }

        /// <inheritdoc />
        public override bool Matches(object value)
        {
            return true;
        }

        /// <inheritdoc />
        protected sealed override bool MatchesCore(T value)
        {
            return true;
        }
    }
}
