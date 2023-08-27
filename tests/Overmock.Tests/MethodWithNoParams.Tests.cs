using Overmocked.Tests.Mocks;
using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests
{
    [TestClass]
    public partial class MethodWithNoParamsTests
    {
        private readonly Model _model1 = new Model();
        private readonly Model _model2 = new Model();
        private readonly List<Model> _models = new List<Model>();

        private IOvermock<IMethodsWithNoParameters> _overmock = null!;

        [TestInitialize]
        public void Initialize()
        {
            _overmock = Overmock.Mock<IMethodsWithNoParameters>();
        }

        [TestMethod]
        public void MethodWith2Params()
        {
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();

            overmock.Mock(p => p.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
                .ToCall(c => c.Get<string>("name"));
            overmock.Target.BoolMethodWithStringAndModel("hello world", _model2);
        }
    }
}