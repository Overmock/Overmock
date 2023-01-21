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
        public static IOvermock<T> Setup<T>(Action<SetupArgs>? argsProvider = null) where T : class
        {
            var result = new Overmock<T>(
                OvermockBuilder.GetTypeBuilder(argsProvider)
            );

            _overmocks.Enqueue(result);

            return result;
        }

        /// <summary>
        /// Verifies the mocks setup behave as expected.
        /// </summary>
        public static void Verify()
        {
            while (_overmocks.TryDequeue(out var verifiable))
            {
                verifiable.Verify();
            }
        }

        internal static void Register<T>(IOvermock<T> overmock) where T : class
        {
            _overmocks.Enqueue(overmock);
        }

        internal static TMethod RegisterMethod<TMethod>(IOvermock overmock, TMethod property) where TMethod : IMethodCall
        {
            return overmock.AddMethod(property);
        }

        internal static TProperty RegisterProperty<TProperty>(IOvermock overmock, TProperty property) where TProperty : IPropertyCall
        {
            return overmock.AddProperty(property);
        }
    }
}