
using System;
using System.Linq.Expressions;

namespace Overmocked
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public static partial class Overmock
    {
        /// <summary>
        /// For the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> OverMock<T, TReturn>(Expression<Func<T, TReturn>> expression) where T : class where TReturn : class
        {
            return Mock<T>().OverMock(expression);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> OverMock<T, TReturn>(T target, Expression<Func<T, TReturn>> expression) where T : class where TReturn : class
        {
            return GetOvermock(target).OverMock(expression);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overmock"></param>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> OverMock<T, TReturn>(IOvermock<T> overmock, Expression<Func<T, TReturn>> expression) where T : class where TReturn : class
        {
            return overmock.OverMock(expression);
        }
    }
}