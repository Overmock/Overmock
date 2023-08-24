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
        /// Sets up the specified <typeparamref name="T" /> type with overmock using the constructor arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An object used to configure overmocks</returns>
        public static T For<T>() where T : class
        {
            return new Overmock<T>(_invocationHandler);
        }

        /// <summary>
        /// Returns an overmocked <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of interface.</typeparam>
        /// <param name="secondaryHandler">The secondary handler.</param>
        /// <returns>The overmocked <typeparamref name="T" />.</returns>
        public static T For<T>(IInvocationHandler? secondaryHandler = null) where T : class
		{
			return new Overmock<T>(secondaryHandler ?? _invocationHandler);
        }
    }
}