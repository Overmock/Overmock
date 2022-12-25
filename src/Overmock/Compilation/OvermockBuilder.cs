namespace Overmock.Compilation
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

        ITypeBuilder IOvermockBuilder.GetTypeBuilder(Action<SetupArgs>? argsProvider)
        {
            return new OvermockTypeBuilder(AssemblyGenerator._instance, argsProvider);
        }
    }
}