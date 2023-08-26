using Kimono;
using System;
using System.Linq.Expressions;

namespace Overmock
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public static partial class Overmock
    {
        /// <summary>
        /// Returns an overmocked <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of interface.</typeparam>
        /// <param name="secondaryHandler">The secondary handler.</param>
        /// <returns>The overmocked <typeparamref name="T" />.</returns>
        public static IOvermock<T> Mock<T>(IInvocationHandler? secondaryHandler = null) where T : class
        {
            return new Overmock<T>(secondaryHandler ?? _invocationHandler);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(Expression<Action<T>> expression) where T : class
        {
            return new Overmock<T>().Mock(expression);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T, TReturn> Mock<T, TReturn>(Expression<Func<T, TReturn>> expression) where T : class
        {
            return new Overmock<T>().Mock(expression);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(T target, Expression<Action<T>> expression) where T : class
        {
            return GetOvermock(target).Mock(expression);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overmock">The overmock.</param>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(IOvermock<T> overmock, Expression<Action<T>> expression) where T : class
        {
            return overmock.Mock(expression);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="overmock">The overmock.</param>
        /// <param name="expression">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T, TReturn> Mock<T, TReturn>(IOvermock<T> overmock, Expression<Func<T, TReturn>> expression) where T : class where TReturn : class
        {
            return overmock.Mock(expression);
        }
    }
}