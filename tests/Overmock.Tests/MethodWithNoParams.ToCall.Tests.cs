namespace Overmocked.Tests
{
    public partial class MethodWithNoParamsTests
    {
        [TestMethod]
        public void VoidMethodWithNoParamsToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.VoidMethodWithNoParams())
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.VoidMethodWithNoParams();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void BoolMethodWithNoParamsToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.BoolMethodWithNoParams())
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.BoolMethodWithNoParams();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ModelMethodWithNoParamsToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.ModelMethodWithNoParams())
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.ModelMethodWithNoParams();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ListOfModelMethodWithNoParamsToCallTest()
        {
            var called = false;

            _overmock.Mock(t => t.ListOfModelMethodWithNoParams())
                .ToCall(c => called = true);

            var target = _overmock.Target;

            Assert.IsNotNull(target);

            target.ListOfModelMethodWithNoParams();

            Assert.IsTrue(called);
        }
    }
}