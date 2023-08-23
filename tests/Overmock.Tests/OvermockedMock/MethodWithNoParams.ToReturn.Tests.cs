namespace Overmock.Tests.OvermockedMock
{
	public partial class MethodWithNoParamsTests
	{
		[TestMethod]
		public void VoidMethodWithNoParamsToBeCalledTest()
		{
            Overmocked.Mock(_overmock, t => t.VoidMethodWithNoParams()).ToBeCalled();

			_overmock.VoidMethodWithNoParams();
		}
		
        [TestMethod]
        public void BoolMethodWithNoParamsToReturnTest()
        {
            Overmocked.Mock(_overmock, t => t.BoolMethodWithNoParams())
                .ToReturn(true);

            var test = _overmock.BoolMethodWithNoParams();

            Assert.IsTrue(test);
		}

		[TestMethod]
		public void ModelMethodWithNoParamsToReturnTest()
		{
			Overmocked.Mock(_overmock, t => t.ModelMethodWithNoParams())
				.ToReturn(_model1);

			var test = _overmock.ModelMethodWithNoParams();

			Assert.AreEqual(_model1, test);
		}

		[TestMethod]
		public void ListOfModelMethodWithNoParamsToReturnTest()
		{
			Overmocked.Mock(_overmock, t => t.ListOfModelMethodWithNoParams())
				.ToReturn(_models);

			var test = _overmock.ListOfModelMethodWithNoParams();

			Assert.AreEqual(_models, test);
		}
	}
}