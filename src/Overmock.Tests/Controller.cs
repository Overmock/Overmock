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

        internal IDidWork<string> DoSomeWork()
        {
            try
            {
                var result = _factory.GoDoYourWork();

                if (result.Result != "testing")
                {
                    return new WrittenWork($"Original: '{result.Result}' with _provider.GetName(): {_provider.GetName()}.");
                }

                return result;
            }
            catch (Exception ex)
            {
                return new WrittenWork(ex.Message);
            }
        }

        internal IDidWork<string> DoSomeWorkWithProperties()
        {
            try
            {
                var factory = _interface.Factory;
                var result = factory.GoDoYourWork();

                if (result.Result != "testing")
                {
                    return new WrittenWork($"Original: '{result.Result}' with _provider.GetName(): {_provider.GetName()}.");
                }

                return result;
            }
            catch (Exception ex)
            {
                return new WrittenWork(ex.Message);
            }
        }
    }
}