namespace Overmock.Tests.OvermockedMock
{
	public partial class MethodWithNoParamsTests
	{
		[TestMethod]
		public void VoidMethodWithNoParamsTest()
		{
			var exception = new Exception();

			Overmocked.Mock(_overmock, t => t.VoidMethodWithNoParams())
				.ToThrow(exception);

			try
			{
				_overmock.VoidMethodWithNoParams();

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

			Overmocked.Mock(_overmock, t => t.BoolMethodWithNoParams())
				.ToThrow(exception);

			try
			{
				_overmock.BoolMethodWithNoParams();

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

			Overmocked.Mock(_overmock, t => t.ModelMethodWithNoParams())
				.ToThrow(exception);

			try
			{
				_overmock.ModelMethodWithNoParams();

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

			Overmocked.Mock(_overmock, t => t.ListOfModelMethodWithNoParams())
				.ToThrow(exception);

			try
			{
				_overmock.ListOfModelMethodWithNoParams();

				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(exception, ex);
			}
		}
	}
}