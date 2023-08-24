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
    public  static partial class Over
    {
        /// <summary>
        /// Mocks the given interface type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MockInterface<T>() where T : class
        {
            return new Overmock<T>();
        }

        /// <summary>
        /// Sets up the specified <typeparamref name="T" /> type with overmock using the constructor arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An object used to configure overmocks</returns>
        public static IOvermock<T> Mock<T>() where T : class
        {
            return new Overmock<T>(_invocationHandler);
        }

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
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(Expression<Action<T>> given) where T : class
        {
            return new Overmock<T>().Mock(given);
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
            return overmock.Mock(given);
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