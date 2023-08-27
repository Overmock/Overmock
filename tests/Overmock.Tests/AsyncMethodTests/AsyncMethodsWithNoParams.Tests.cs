using Overmocked.Tests.Mocks.Methods.Async;

namespace Overmocked.Tests
{
    [TestClass]
    public partial class AsyncMethodsWithNoParamsTests
    {
        private readonly IOvermock<IAsyncMethodsWithNoParams> _overmock = new Overmock<IAsyncMethodsWithNoParams>();

        [TestInitialize]
        public void Initialize()
        {

        }
    }
}
