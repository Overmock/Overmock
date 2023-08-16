﻿using Kimono;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Overmock
{
	/// <summary>
	/// Contains methods used for configuring an overmock.
	/// </summary>
	public static class Overmocked
	{
		private static readonly ConcurrentQueue<IOvermock> _overmocks = new ConcurrentQueue<IOvermock>();
        
        private static IInvocationHandler? _invocationHandler;

		/// <summary>
		/// Initializes static members of the <see cref="Overmocked"/> class.
		/// </summary>
		static Overmocked()
        {
        }

        /// <summary>
        /// Uses the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public static void Use(IInvocationHandler handler)
        {
            Interlocked.Exchange(ref _invocationHandler, handler);
        }

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

		/// <summary>
		/// Sets up the specified <typeparamref name="T" /> type with overmock using the constructor arguments.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>An object used to configure overmocks</returns>
		public static IOvermock<T> Overmock<T>() where T : class
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

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T> Mock<T>(T target, Expression<Action<T>> given) where T : class
        {
            Overmock<T>? overmock = null;

            if (target is Overmock<T> mock)
            {
                overmock = mock;
            }

            if (IsRegistered(target))
            {
                overmock = (Overmock<T>)GetRegistration(target)!;
            }

            if (overmock is null)
            {
                overmock = new Overmock<T>(_invocationHandler);
            }

            return overmock.Override(given);
        }

        /// <summary>
        /// Fors the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="given">The use.</param>
        /// <returns>ISetup&lt;T&gt;.</returns>
        public static ISetup<T, TReturn> Mock<T, TReturn>(T target, Expression<Func<T, TReturn>> given) where T : class
        {
            Overmock<T>? overmock = null;

            if (target is Overmock<T> mock)
            {
                overmock = mock;
            }

            if (IsRegistered(target))
            {
                overmock = (Overmock<T>)GetRegistration(target)!;
            }

            if (overmock is null)
            {
                overmock = new Overmock<T>();
            }

            return overmock.Override(given);
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
        public static ISetup<T, TReturn> Mock<T, TReturn>(IOvermock<T> overmock, Expression<Func<T, TReturn>> given) where T : class
        {
            return overmock.Override(given);
        }

        /// <summary>
        /// Verifies the mocks setup behave as expected.
        /// </summary>
        public static void Verify()
		{
            foreach (var overmock in _overmocks)
            {
                overmock.Verify();
            }
        }

		internal static void Register<T>(IOvermock<T> overmock) where T : class
        {
            _overmocks.Enqueue(overmock);
        }

        internal static bool IsRegistered<T>(T target) where T : class
        {
            return _overmocks.Any(o => o.GetTarget() == target);
        }

        internal static IOvermock? GetRegistration<T>(T target) where T : class
        {
            return _overmocks.FirstOrDefault(o => o.GetTarget() == target);
        }

        internal static IOvermock? GetRegistration<T>(IOvermock<T> target) where T : class
        {
            return _overmocks.FirstOrDefault(o => o == target);
        }

        internal static IMethodCall<T> RegisterMethod<T>(IOvermock overmock, IMethodCall<T> method) where T : class
		{
			return overmock.AddMethod(method);
		}

		internal static IMethodCall<T, TReturn> RegisterMethod<T, TReturn>(IOvermock overmock, IMethodCall<T, TReturn> method) where T : class
		{
			return overmock.AddMethod(method);
		}

		internal static IPropertyCall<T, TProperty> RegisterProperty<T, TProperty>(IOvermock overmock, IPropertyCall<T, TProperty> property) where T : class
		{
			return overmock.AddProperty(property);
		}
    }
}