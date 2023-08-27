﻿namespace Overmocked.Tests.OvermockedMock
{
    public partial class PropertyGetTests
    {
        //[TestMethod]
        //public void IntPropertyToCallTest()
        //{
        //    var called = false;

        //    Overmock.Mock(_overmock, t => t.Int)
        //        .ToCall(c => called = true);

        //    Overmock.Mock(_overmock, t => t.GetHashCode()).ToBeCalled();

        //    var model = _overmock.Int;

        //    Assert.ThrowsException<UnhandledMemberException>(() => _overmock.Equals(null));

        //    Assert.IsTrue(called);
        //}

        [TestMethod]
        public void StringPropertyToCallTest()
        {
            var called = false;

            Overmock.OverMock(_overmock, t => t.String)
                .ToCall(c => called = true);

            var model = _overmock.Target.String;

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ModelPropertyToCallTest()
        {
            var called = false;

            Overmock.OverMock(_overmock, t => t.Model)
                .ToCall(c => called = true);

            var model = _overmock.Target.Model;

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ListOfModelPropertyToCallTest()
        {
            var called = false;

            Overmock.OverMock(_overmock, t => t.ListOfModels)
                .ToCall(c => called = true);

            var model = _overmock.Target.ListOfModels;

            Assert.IsTrue(called);
        }
    }
}
