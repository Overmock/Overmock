using Overmock.Compilation.Roslyn;

namespace Overmock
{
	/// <summary>
	/// OvermockBuilder is the beginning point of compilation and sets the <see cref="ITypeBuilder"/> used in type generation.
	/// </summary>
	public static class OvermockBuilder
    {
        private static readonly object _syncRoot = new();
        private static volatile IOvermockBuilder _instance;

        static OvermockBuilder()
        {
            _instance = new RoslynOvermockBuilder();
        }

        /// <summary>
        /// The singleton instance used to construct type builders.
        /// </summary>
        public static IOvermockBuilder Instance => _instance;

        /// <summary>
        /// Sets the current <see cref="Instance"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UseBuilder(IOvermockBuilder instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            lock (_syncRoot)
			{
				_instance = instance;
			}
        }

		/// <summary>
		/// Delegates to <see cref="Instance"/> to generate the <see cref="ITypeBuilder" />.
		/// </summary>
		/// <param name="argsProvider"></param>
		/// <returns>An <see cref="ITypeBuilder" /> instance.</returns>
		public static ITypeBuilder GetTypeBuilder(Action<SetupArgs>? argsProvider)
        {
            return Instance.GetTypeBuilder(argsProvider);
        }
	}
}