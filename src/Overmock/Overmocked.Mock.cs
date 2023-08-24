using Kimono;
using Kimono.Proxies;
using Overmock.Mocking;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Overmock
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public  static partial class Overmocked
	{
        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overmock">The overmock.</param>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static IOvermock<T> Overmock<T>() where T : class
        {
            return new Overmock<T>();
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(Expression<Action<T>> given) where T : class
        {
            return Overmock<T>().Override(given);
        }

        /// <summary>
        /// For the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> Mock<T, TReturn>(Expression<Func<T, TReturn>> given) where T : class where TReturn : class
        {
            return Overmock<T>().Mock(given);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(T target, Expression<Action<T>> given) where T : class
        {
            return GetOvermock(target).Override(given);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetupMocks<T, TReturn> Mock<T, TReturn>(T target, Expression<Func<T, TReturn>> given) where T : class where TReturn : class
        {
            return GetOvermock(target).Mock(given);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overmock">The overmock.</param>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(IOvermock<T> overmock, Expression<Action<T>> given) where T : class
        {
            return overmock.Override(given);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="overmock">The overmock.</param>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T, TReturn> Mock<T, TReturn>(IOvermock<T> overmock, Expression<Func<T, TReturn>> given) where T : class where TReturn : class
        {
            return overmock.Mock(given);
        }
    }
}