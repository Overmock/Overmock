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

            Over.Overmock(_overmock, t => t.String)
                .ToThrow(exception);

            try
            {
                var model = _overmock.String;

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

            Over.Overmock(_overmock, t => t.Model)
                .ToThrow(exception);

            try
            {
                var model = _overmock.Model;

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

            Over.Overmock(_overmock, t => t.ListOfModels)
                .ToThrow(exception);

            try
            {
                var model = _overmock.ListOfModels;

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception, ex);
            }
        }
    }
}
