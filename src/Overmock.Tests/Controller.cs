using Overmock.Tests.Mocks;

namespace Overmock.Tests
{
    internal class Controller
    {
        private ITestInterface _interface;
        private Factory _factory;
        private IProvider _provider;

        public Controller(ITestInterface testIteraface, Factory factory, IProvider provider)
        {
            _interface = testIteraface;
            _factory = factory;
            _provider = provider;
        }

        internal ITestInterface Interface => _interface;

        internal Factory Factory => _factory;

        internal IProvider Provider => _provider;

        internal void DoSomeWork()
        {
            throw new NotImplementedException();
        }
    }
}