using System.Collections.Concurrent;

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
		private static readonly ConcurrentQueue<IVerifiable> _overmocks = new ConcurrentQueue<IVerifiable>();

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
		/// <param name="value">if set to <c>true</c> [value].</param>
		/// <returns>IOvermock&lt;T&gt;.</returns>
		public static IOvermock<T> ExpectAnyInvocation<T>(bool value = true) where T : class
		{
			var result = new Overmock<T>();

			_overmocks.Enqueue(result);

			((IExpectAnyInvocation)result).ExpectAny(value);

			return result;
		}


		/// <summary>
		/// Sets up the specified <typeparamref name="T" /> type with overmock using the constructor arguments.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="argsProvider">The arguments provider.</param>
		/// <returns>An object used to configure overmocks</returns>
		public static IOvermock<T> Interface<T>(Action<SetupArgs>? argsProvider = null) where T : class
		{
			var result = new Overmock<T>(argsProvider: argsProvider);

			_overmocks.Enqueue(result);

			return result;
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

		/// <summary>
		/// Registers the specified overmock.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="overmock">The overmock.</param>
		internal static void Register<T>(IOvermock<T> overmock) where T : class
        {
            _overmocks.Enqueue(overmock);
        }

		/// <summary>
		/// Registers the method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="overmock">The overmock.</param>
		/// <param name="method">The method.</param>
		/// <returns>IMethodCall&lt;T&gt;.</returns>
		internal static IMethodCall<T> RegisterMethod<T>(IOvermock overmock, IMethodCall<T> method) where T : class
		{
			return overmock.AddMethod(method);
		}

		/// <summary>
		/// Registers the method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TReturn">The type of the t return.</typeparam>
		/// <param name="overmock">The overmock.</param>
		/// <param name="method">The method.</param>
		/// <returns>IMethodCall&lt;T, TReturn&gt;.</returns>
		internal static IMethodCall<T, TReturn> RegisterMethod<T, TReturn>(IOvermock overmock, IMethodCall<T, TReturn> method) where T : class
		{
			return overmock.AddMethod(method);
		}

		/// <summary>
		/// Registers the property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TProperty">The type of the t property.</typeparam>
		/// <param name="overmock">The overmock.</param>
		/// <param name="property">The property.</param>
		/// <returns>IPropertyCall&lt;T, TProperty&gt;.</returns>
		internal static IPropertyCall<T, TProperty> RegisterProperty<T, TProperty>(IOvermock overmock, IPropertyCall<T, TProperty> property) where T : class
		{
			return overmock.AddProperty(property);
		}
	}
}