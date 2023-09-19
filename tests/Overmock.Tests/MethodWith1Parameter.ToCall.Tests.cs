namespace Overmocked.Tests
{
    public partial class MethodWith1Parameter
    {
        [TestMethod]
        public void VoidMethodWithInt_ToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.VoidMethodWithInt(Its.Any<int>()))
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.VoidMethodWithInt(52);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void BoolMethodWithString_ToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.BoolMethodWithString(Its.Any<string>()))
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.BoolMethodWithString("name");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ModelMethodWithFuncOfListOfModel_ToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.ModelMethodWithFuncOfListOfModel(Its.This(52)))
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.ModelMethodWithFuncOfListOfModel(52);

            Assert.IsTrue(called);
        }

        //[TestMethod]
        //public void ListOfModelMethodWithDecimal_ToCallTest()
        //{
        //    var called = false;

        //    _overmock.Mock(t => t.ListOfModelMethodWithDecimal(Its.Any<decimal>()))
        //        .ToCall(c => {
        //            called = true;
        //            return _models;
        //        });

        //    var target = _overmock.Target;

        //    Assert.IsNotNull(target);

        //    var list = target.ListOfModelMethodWithDecimal(2.0m);

        //    Assert.IsTrue(called);
        //    Assert.AreEqual(_models, list);
        //}
    }
}