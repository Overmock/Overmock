using System.Collections.Concurrent;

namespace Overmock
{
    public static class Overmocked
    {
        private static readonly ConcurrentQueue<IVerifiable> Verifiables = new ConcurrentQueue<IVerifiable>();

        static Overmocked()
        {
        }

        internal static IOvermockBuilder Builder => OvermockBuilder.Instance;

        /// <summary>
        /// Setups the specified <typeparamref name="T" /> up with overmock using the constructor arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argsProvider">The arguments provider.</param>
        /// <returns></returns>
        public static IOvermock<T> Setup<T>(Action<SetupArgs>? argsProvider = null) where T : class
        {
            var result = new Overmock<T>(Builder.GetTypeBuilder(argsProvider));

            Verifiables.Enqueue(result);

            return result;
        }

        /// <summary>
        /// Verifies the mocks setup behave as expected.
        /// </summary>
        public static void Verify()
        {
            while (Verifiables.TryDequeue(out var verifiable))
            {
                verifiable.Verify();
            }
        }

        internal static void Register<T>(IOvermock<T> overmock) where T : class
        {
            Verifiables.Enqueue(overmock);
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