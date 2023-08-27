using Overmocked.Tests.Mocks;
using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests
{
    [TestClass]
    public partial class MethodWith1Parameter
    {
        private readonly Model _model1 = new Model();
        private readonly Model _model2 = new Model();
        private readonly List<Model> _models = new List<Model>();

        private IOvermock<IMethodsWith1Parameter> _overmock = null!;

        [TestInitialize]
        public void Initialize()
        {
            _overmock = Overmock.Mock<IMethodsWith1Parameter>();
        }
    }
}