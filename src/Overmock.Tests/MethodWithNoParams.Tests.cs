using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
{
    [TestClass]
	public partial class MethodWithNoParamsTests
	{
		private readonly Model _model1 = new Model();
		private readonly Model _model2 = new Model();
		private readonly List<Model> _models = new List<Model>();

		private IOvermock<IMethodsWithNoParameters> _testInterface = null!;

		[TestInitialize]
		public void Initialize()
		{
            _testInterface = Overmocked.Interface<IMethodsWithNoParameters>();
		}

		[TestMethod]
		public void MethodWith2Params()
		{
			var overmock = Overmocked.Interface<IMethodsWith2Parameters>();

			overmock.Override(p => p.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
				.ToCall(c => c.Get<string>("name"));
			overmock.Target.BoolMethodWithStringAndModel("hello world", _model2);
		}
	}
}