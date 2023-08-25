namespace Overmock.Tests.OvermockedMock
{
    public partial class PropertyGetTests
    {
        //[TestMethod]
        //public void IntPropertyToThrowTest()
        //{
        //	var exception = new Exception();

        //	Overmocked.Mock(_overmock, t => t.Int)
        //		.ToThrow(exception);

        //	Assert.IsNotNull(_overmock);

        //	try
        //	{
        //		var model = _overmock.Int;

        //		Assert.Fail();
        //	}
        //	catch (Exception ex)
        //	{
        //		Assert.AreEqual(exception, ex);
        //	}
        //}

        [TestMethod]
        public void StringPropertyToThrowTest()
        {
            var exception = new Exception();

            Overmock.OverMock(_overmock, t => t.String)
                .ToThrow(exception);

            try
            {
                var model = _overmock.Target.String;

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void ModelPropertyToThrowTest()
        {
            var exception = new Exception();

            Overmock.OverMock(_overmock, t => t.Model)
                .ToThrow(exception);

            try
            {
                var model = _overmock.Target.Model;

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }

        [TestMethod]
        public void ListOfModelPropertyToThrowTest()
        {
            var exception = new Exception();

            Overmock.OverMock(_overmock, t => t.ListOfModels)
                .ToThrow(exception);

            try
            {
                var model = _overmock.Target.ListOfModels;

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }
    }
}
