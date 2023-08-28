using Overmocked.Tests.Mocks;

namespace Overmocked.Tests
{
    public partial class MethodWith1Parameter
    {
        [TestMethod]
        public void VoidMethodWithNoParamsToBeCalledTest()
        {
            _overmock.Mock(t => t.VoidMethodWithInt(Its.Any<int>())).ToBeCalled();

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.VoidMethodWithInt(52);
        }

        [TestMethod]
        public void BoolMethodWithString_ToReturnTest()
        {
            _overmock.Mock(t => t.BoolMethodWithString(Its.Any<string>()))
                .ToReturn(true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            var test = target.BoolMethodWithString("");

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void ModelMethodWithFuncOfListOfModel_ToReturnTest()
        {
            _overmock.Mock(t => t.ModelMethodWithFuncOfListOfModel(Its.Any<Func<List<Model>>>()))
                .ToReturn(_model1);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            var test = target.ModelMethodWithFuncOfListOfModel(() => _models);

            Assert.AreEqual(_model1, test);
        }

        //[TestMethod]
        //public void ListOfModelMethodWithDecimal_ToReturnTest()
        //{
        //    _overmock.Mock(t => t.ListOfModelMethodWithDecimal(Its.Any<decimal>()))
        //        .ToReturn(_models);

        //    var target = _overmock.Target;

        //    Assert.IsNotNull(target);

        //    var test = target.ListOfModelMethodWithDecimal(5.2m);

        //    Assert.AreEqual(_models, test);
        //}
    }
}