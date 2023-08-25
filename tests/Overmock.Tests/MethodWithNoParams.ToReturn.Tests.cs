namespace Overmock.Tests
{
    public partial class MethodWithNoParamsTests
    {
        [TestMethod]
        public void VoidMethodWithNoParamsToBeCalledTest()
        {
            _overmock.Mock(t => t.VoidMethodWithNoParams()).ToBeCalled();

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.VoidMethodWithNoParams();
        }

        [TestMethod]
        public void BoolMethodWithNoParamsToReturnTest()
        {
            _overmock.Mock(t => t.BoolMethodWithNoParams())
                .ToReturn(true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            var test = target.BoolMethodWithNoParams();

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void ModelMethodWithNoParamsToReturnTest()
        {
            _overmock.Mock(t => t.ModelMethodWithNoParams())
                .ToReturn(_model1);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            var test = target.ModelMethodWithNoParams();

            Assert.AreEqual(_model1, test);
        }

        [TestMethod]
        public void ListOfModelMethodWithNoParamsToReturnTest()
        {
            _overmock.Mock(t => t.ListOfModelMethodWithNoParams())
                .ToReturn(_models);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            var test = target.ListOfModelMethodWithNoParams();

            Assert.AreEqual(_models, test);
        }
    }
}