namespace Overmock.Tests.OvermockedMock
{
    public partial class MethodWithNoParamsTests
    {
        [TestMethod]
        public void VoidMethodWithNoParamsToCallTest()
        {
            var called = false;

            Over.Mock(_overmock, t => t.VoidMethodWithNoParams())
                .ToCall(c => called = true);

            _overmock.VoidMethodWithNoParams();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void BoolMethodWithNoParamsToCallTest()
        {
            var called = false;

            Over.Mock(_overmock, t => t.BoolMethodWithNoParams())
                .ToCall(c => called = true);

            _overmock.BoolMethodWithNoParams();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ModelMethodWithNoParamsToCallTest()
        {
            var called = false;

            Over.Overmock(_overmock, t => t.ModelMethodWithNoParams())
                .ToCall(c => called = true);

            _overmock.ModelMethodWithNoParams();

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ListOfModelMethodWithNoParamsToCallTest()
        {
            var called = false;

            Over.Overmock(_overmock, t => t.ListOfModelMethodWithNoParams())
                .ToCall(c => called = true);

            _overmock.ListOfModelMethodWithNoParams();

            Assert.IsTrue(called);
        }
    }
}