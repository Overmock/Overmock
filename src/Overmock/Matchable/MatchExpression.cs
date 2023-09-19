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
            return _value.Invoke(other);
        }

        /// <inheritdoc />
        public virtual bool Matches(T value)
        {
            return _value.Invoke(value);
        }

        /// <inheritdoc />
        public bool Matches(object value)
        {
            if (value is T)
            {
                return Matches((T)value);
            }

            return false;
        }
    }
}