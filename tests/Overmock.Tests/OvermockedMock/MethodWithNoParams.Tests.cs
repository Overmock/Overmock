using Overmocked.Tests.Mocks;
using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests.OvermockedMock
{
    [TestClass]
    public partial class MethodWithNoParamsTests
    {
        private readonly Model _model1 = new Model();
        private readonly Model _model2 = new Model();
        private readonly List<Model> _models = new List<Model>();

        private IMethodsWithNoParameters _overmock = null!;

        [TestInitialize]
        public void Initialize()
        {
            _overmock = Overmock.Interface<IMethodsWithNoParameters>();
        }
    }
}