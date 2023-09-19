using System;
using System.Linq.Expressions;
using Overmocked.Matchable;

namespace Overmocked
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Matches(object value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TMatch"></typeparam>
    public interface IMatch<T> : IMatch, IFluentInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Matches(T value);
    }

    /// <summary>
    /// Represents values for mocked methods.
    /// </summary>
    public static class Its
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Any&lt;T&gt;.</returns>
        public static Any<T> Any<T>()
        {
            return new Any<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>This&lt;T&gt;.</returns>
        public static This<T> This<T>(T value)
        {
            return new This<T>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MatchExpression<T> Matching<T>(Expression<Func<T, bool>> value)
        {
            return new MatchExpression<T>(value);
        }
    }
}
