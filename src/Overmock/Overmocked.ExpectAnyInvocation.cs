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
		/// Signals the Overmock to expect any invocation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>IOvermock&lt;T&gt;.</returns>
		public static IOvermock<T> ExpectAnyInvocation<T>() where T : class
		{
			var result = new Overmock<T>();

			((IExpectAnyInvocation)result).ExpectAny(true);

			return result;
        }

        ///// <summary>
        ///// Signals the Overmock to expect any invocation.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="target">The target.</param>
        ///// <returns>IOvermock&lt;T&gt;.</returns>
        //public static T ExpectAnyInvocation<T>(T target) where T : class
        //{
        //    IOvermock<T> overmock = GetOvermock(target);

        //    ExpectAnyInvocation(overmock);

        //    return target;
        //}
    }
}