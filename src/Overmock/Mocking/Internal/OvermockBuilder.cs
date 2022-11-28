namespace Overmock.Mocking.Internal
{
    internal class OvermockBuilder : IOvermockBuilder
    {
        private static volatile IOvermockBuilder _instance;

        private OvermockBuilder() { }

        static OvermockBuilder()
        {
            _instance = new OvermockBuilder();
        }

        public static IOvermockBuilder Instance => _instance;

        ITypeBuilder IOvermockBuilder.GetTypeBuilder(Action<SetupArgs>? argsProvider = null)
        {
            return new OvermockTypeBuilder(AssemblyGenerator.Instance, argsProvider);
        }
    }
}