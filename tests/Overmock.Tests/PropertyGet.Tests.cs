using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Properties;

namespace Overmock.Tests
{
    [TestClass]
    public partial class PropertyGetTests
	{
		private readonly IOvermock<IPropertiesWithGet> _overmock = Over.Mock<IPropertiesWithGet>();

		private readonly Model _model1 = new Model();
		private readonly Model _model2 = new Model();
		private readonly List<Model> _models = new List<Model>();

		[TestInitialize]
		public void Initialize()
		{
			_models.Add(_model1);
			_models.Add(_model2);
		}

		[TestMethod]
		public void Test()
		{
            var overmock = Over.Mock<Model>();

            overmock.Mock(m => m.Id)
                .ToReturn(420);

            var target = overmock.Target;

            Assert.IsNotNull(target);

            Assert.AreEqual(420, target.Id);
		}
	}
}
