namespace Overmock.Tests
{
    public partial class MethodWithNoParamsTests
    {
        [TestMethod]
        public void VoidMethodWithNoParamsTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.VoidMethodWithNoParams())
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.VoidMethodWithNoParams();

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void BoolMethodWithNoParamsTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.BoolMethodWithNoParams())
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.BoolMethodWithNoParams();

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void ModelMethodWithNoParamsTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.ModelMethodWithNoParams())
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.ModelMethodWithNoParams();

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void ListOfModelMethodWithNoParamsTest()
        {
            var exception = new Exception();

            _overmock.Mock(t => t.ListOfModelMethodWithNoParams())
                .ToThrow(exception);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            try
            {
                target.ListOfModelMethodWithNoParams();

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }
    }
}