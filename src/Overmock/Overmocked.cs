using System.Collections.Concurrent;

namespace Overmock
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public static class Overmocked
	{
		private static readonly ConcurrentQueue<IVerifiable> _overmocks = new ConcurrentQueue<IVerifiable>();

		static Overmocked()
		{
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

        internal static void Register<T>(IOvermock<T> overmock) where T : class
        {
            _overmocks.Enqueue(overmock);
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