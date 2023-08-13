namespace Overmock.Tests
{
	public partial class MethodWithNoParamsTests
	{
		[TestMethod]
		public void VoidMethodWithNoParamsToBeCalledTest()
		{
            _testInterface.Override(t => t.VoidMethodWithNoParams()).ToBeCalled();

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			target.VoidMethodWithNoParams();
		}
		
        [TestMethod]
        public void BoolMethodWithNoParamsToReturnTest()
        {
            _testInterface.Override(t => t.BoolMethodWithNoParams())
                .ToReturn(true);

            var target = _testInterface.Target;

            Assert.IsNotNull(target);

            var test = target.BoolMethodWithNoParams();

            Assert.IsTrue(test);
		}

		[TestMethod]
		public void ModelMethodWithNoParamsToReturnTest()
		{
			_testInterface.Override(t => t.ModelMethodWithNoParams())
				.ToReturn(_model1);

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			var test = target.ModelMethodWithNoParams();

			Assert.AreEqual(_model1, test);
		}

		[TestMethod]
		public void ListOfModelMethodWithNoParamsToReturnTest()
		{
			_testInterface.Override(t => t.ListOfModelMethodWithNoParams())
				.ToReturn(_models);

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			var test = target.ListOfModelMethodWithNoParams();

			Assert.AreEqual(_models, test);
		}
	}
}