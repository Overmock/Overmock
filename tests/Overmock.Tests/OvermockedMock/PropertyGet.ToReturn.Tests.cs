namespace Overmocked.Tests.OvermockedMock
{
    public partial class PropertyGetTests
    {
        //[TestMethod]
        //public void IntPropertyToReturnTest()
        //{
        //    Overmock.Mock(_overmock, t => t.Int)
        //        .ToReturn(20);

        //    var test = _overmock.Int;

        //    Assert.AreEqual(20, test);
        //}

        [TestMethod]
        public void StringPropertyToReturnTest()
        {
            Overmock.OverMock(_overmock, t => t.String)
                .ToReturn("testing-name");

            var test = _overmock.Target.String;

            Assert.AreEqual("testing-name", test);
        }

        [TestMethod]
        public void ModelPropertyToReturnTest()
        {
            Overmock.OverMock(_overmock, t => t.Model)
                .ToReturn(_model1);

            var test = _overmock.Target.Model;

            Assert.AreEqual(_model1, test);
        }

        [TestMethod]
        public void ListOfModelPropertyToReturnTest()
        {
            Overmock.OverMock(_overmock, t => t.ListOfModels)
                .ToReturn(_models);

            var test = _overmock.Target.ListOfModels;

            Assert.AreEqual(_models, test);
        }
    }
}
