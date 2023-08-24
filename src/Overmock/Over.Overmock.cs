
using System.Linq.Expressions;
using System;

namespace Overmock
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public  static partial class Over
    {
        /// <summary>
        /// For the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> Overmock<T, TReturn>(Expression<Func<T, TReturn>> given) where T : class where TReturn : class
        {
            return Mock<T>().Overmock(given);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> Overmock<T, TReturn>(T target, Expression<Func<T, TReturn>> given) where T : class where TReturn : class
        {
            return GetOvermock(target).Overmock(given);
        }

    }
}