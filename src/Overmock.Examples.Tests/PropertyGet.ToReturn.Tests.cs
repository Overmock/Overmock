using Overmock.Examples.Tests.Methods;
using Overmock.Examples.Tests.Properties;

namespace Overmock.Examples.Tests
{
    [TestClass]
    public class PropertyGetToReturnTests
	{
		private readonly IOvermock<IPropertiesWithGet> _overmock = Overmocked.Setup<IPropertiesWithGet>();

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
		public void IntPropertyTest()
		{
			_overmock.Override(t => t.Int)
				.ToReturn(20);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.Int;

			Assert.AreEqual(20, test);
		}

		[TestMethod]
		public void StringPropertyTest()
		{
			_overmock.Override(t => t.String)
				.ToReturn("testing-name");

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.String;

			Assert.AreEqual("testing-name", test);
		}

		[TestMethod]
		public void ModelPropertyTest()
		{
			_overmock.Override(t => t.Model)
				.ToReturn(_model1);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.Model;

			Assert.AreEqual(_model1, test);
		}

		[TestMethod]
		public void ListOfModelPropertyTest()
		{
			_overmock.Override(t => t.ListOfModels)
				.ToReturn(_models);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.ListOfModels;

			Assert.AreEqual(_models, test);
		}
	}
}
