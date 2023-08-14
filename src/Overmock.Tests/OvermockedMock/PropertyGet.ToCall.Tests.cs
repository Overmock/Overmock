namespace Overmock.Tests.OvermockedMock
{
	public partial class PropertyGetTests
	{
		[TestMethod]
		public void IntPropertyToCallTest()
		{
			var called = false;

            Overmocked.Mock(_overmock, t => t.Int)
				.ToCall(c => called = true);

            Overmocked.Mock(_overmock, t => t.GetHashCode()).ToBeCalled();

			var model = _overmock.Int;

			Assert.ThrowsException<UnhandledMemberException>(() => _overmock.Equals(null));

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void StringPropertyToCallTest()
		{
			var called = false;

			Overmocked.Mock(_overmock, t => t.String)
				.ToCall(c => called = true);

			var model = _overmock.String;

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ModelPropertyToCallTest()
		{
			var called = false;

			Overmocked.Mock(_overmock, t => t.Model)
				.ToCall(c => called = true);

			var model = _overmock.Model;

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ListOfModelPropertyToCallTest()
		{
			var called = false;

			Overmocked.Mock(_overmock, t => t.ListOfModels)
				.ToCall(c => called = true);

			var model = _overmock.ListOfModels;

			Assert.IsTrue(called);
		}
	}
}
