namespace Overmock.Tests
{
	public partial class PropertyGetTests
	{
		[TestMethod]
		public void IntPropertyToCallTest()
		{
			var called = false;

			_overmock.Override(t => t.Int)
				.ToCall(c => called = true);

			_overmock.Override(t => t.GetHashCode()).ToBeCalled();

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var model = target.Int;

			Assert.ThrowsException<UnhandledMemberException>(() => target.Equals(null));

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void StringPropertyToCallTest()
		{
			var called = false;

			_overmock.Override(t => t.String)
				.ToCall(c => called = true);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var model = target.String;

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ModelPropertyToCallTest()
		{
			var called = false;

			_overmock.Override(t => t.Model)
				.ToCall(c => called = true);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var model = target.Model;

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void ListOfModelPropertyToCallTest()
		{
			var called = false;

			_overmock.Override(t => t.ListOfModels)
				.ToCall(c => called = true);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			var model = target.ListOfModels;

			Assert.IsTrue(called);
		}
	}
}
