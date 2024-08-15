using System;
using System.Linq.Expressions;

namespace Overmocked.Matchable
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatchExpression<T> : ParameterValue<T>, IMatch<T>
    {
        private readonly Func<T, bool> _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public MatchExpression(Expression<Func<T, bool>> value)
        {
            _value = value.Compile();
        }

        /// <inheritdoc />
        public override bool Equals(T other)
        {
            return MatchesCore(other);
        }

        /// <inheritdoc />
        public bool Matches(T value)
        {
            return MatchesCore(value);
        }

        /// <inheritdoc />
        public virtual bool Matches(object value)
        {
            if (value is T t)
            {
                return MatchesCore(t);
            }

            if (value is null)
            {
                return MatchesCore(default!);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual bool MatchesCore(T value)
        {
            return _value.Invoke(value);
        }
    }
}