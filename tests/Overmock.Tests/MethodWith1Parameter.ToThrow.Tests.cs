using Overmocked.Tests.Mocks;

namespace Overmocked.Tests
{
    public partial class MethodWith1Parameter
    {
        [TestMethod]
        public void VoidMethodWithInt_ToThrowTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.VoidMethodWithInt(Its.Any<int>()))
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.VoidMethodWithInt(52);

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void BoolMethodWithString_ToThrowTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.BoolMethodWithString(Its.Any<string>()))
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.BoolMethodWithString("hello");

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void ModelMethodWithFuncOfListOfModel_ToThrowTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.ModelMethodWithFuncOfListOfModel(Its.Any<Func<List<Model>>>()))
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.ModelMethodWithFuncOfListOfModel(() => _models);

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        //[TestMethod]
        //public void ListOfModelMethodWithDecimal_ToThrowTest()
        //{
        //    var exception = new Exception();

        //    _overmock.Mock(t => t.ListOfModelMethodWithDecimal(Its.Any<decimal>()))
        //        .ToThrow(exception);

        //    var target = _overmock.Target;

        //    Assert.IsNotNull(target);

        //    try
        //    {
        //        target.ListOfModelMethodWithDecimal(52);

        //        Assert.Fail();
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.AreEqual(exception, ex);
        //    }
        //}
    }
}