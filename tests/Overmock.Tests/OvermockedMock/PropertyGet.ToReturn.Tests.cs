namespace Overmock.Tests.OvermockedMock
{
	public partial class PropertyGetTests
    {
        //[TestMethod]
        //public void IntPropertyToReturnTest()
        //{
        //    Overmocked.Mock(_overmock, t => t.Int)
        //        .ToReturn(20);

        //    var test = _overmock.Int;

        //    Assert.AreEqual(20, test);
        //}

        [TestMethod]
		public void StringPropertyToReturnTest()
		{
			Over.Overmock(_overmock, t => t.String)
				.ToReturn("testing-name");

			var test = _overmock.String;

			Assert.AreEqual("testing-name", test);
		}

		[TestMethod]
		public void ModelPropertyToReturnTest()
		{
			Over.Overmock(_overmock, t => t.Model)
				.ToReturn(_model1);

			var test = _overmock.Model;

			Assert.AreEqual(_model1, test);
		}

		[TestMethod]
		public void ListOfModelPropertyToReturnTest()
		{
            Over.Overmock(_overmock, t => t.ListOfModels)
				.ToReturn(_models);

			var test = _overmock.ListOfModels;

			Assert.AreEqual(_models, test);
		}
	}
}
