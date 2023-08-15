using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Overmock
{
	/// <summary>
	/// Contains methods used for configuring an overmock.
	/// </summary>
	public static class Overmocked
	{
		/// <summary>
		/// The overmocks
		/// </summary>
		private static readonly ConcurrentQueue<IOvermock> _overmocks = new ConcurrentQueue<IOvermock>();

		/// <summary>
		/// Initializes static members of the <see cref="Overmocked"/> class.
		/// </summary>
		static Overmocked()
		{
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
            return new Overmock<T>();
        }

        /// <summary>
        /// Returns an overmocked <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of interface.</typeparam>
        /// <returns>The overmocked <typeparamref name="T" />.</returns>
        public static T For<T>() where T : class
		{
			return new Overmock<T>();
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
                overmock = new Overmock<T>();
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
            foreach (var verifiable in _overmocks)
            {
                verifiable.Verify();
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