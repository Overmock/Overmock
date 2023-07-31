using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Properties;

namespace Overmock.Tests
{
    public partial class PropertyGetTests
	{
		[TestMethod]
		public void IntPropertyToReturnTest()
		{
			_overmock.Override(t => t.Int)
				.ToReturn(20);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.Int;

			Assert.AreEqual(20, test);
		}

		[TestMethod]
		public void StringPropertyToReturnTest()
		{
			_overmock.Override(t => t.String)
				.ToReturn("testing-name");

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.String;

			Assert.AreEqual("testing-name", test);
		}

		[TestMethod]
		public void ModelPropertyToReturnTest()
		{
			_overmock.Override(t => t.Model)
				.ToReturn(_model1);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var test = target.Model;

			Assert.AreEqual(_model1, test);
		}

		[TestMethod]
		public void ListOfModelPropertyToReturnTest()
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
