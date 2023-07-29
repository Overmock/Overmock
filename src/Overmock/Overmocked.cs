using System.Collections.Concurrent;
using Overmock.Runtime.Marshalling;

namespace Overmock
{
	/// <summary>
	/// Contains methods used for configuring an overmock.
	/// </summary>
	public static class Overmocked
	{
		private static readonly ConcurrentQueue<IVerifiable> _overmocks = new ConcurrentQueue<IVerifiable>();

        private static readonly IMarshallerFactory _marshallerFactory = new MarshallerFactory();

		static Overmocked()
		{
		}

		/// <summary>
		/// Sets up the specified <typeparamref name="T" /> type with overmock using the constructor arguments.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="argsProvider">The arguments provider.</param>
		/// <returns>An object used to configure overmocks</returns>
		public static IOvermock<T> Setup<T>(Action<SetupArgs>? argsProvider = null) where T : class
		{
			var result = new Overmock<T>();

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
		
        internal static IMarshallerFactory GetMarshallerFactory()
        {
            return _marshallerFactory;
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