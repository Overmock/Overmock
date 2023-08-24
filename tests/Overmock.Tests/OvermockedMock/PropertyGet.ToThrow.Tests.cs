namespace Overmock.Tests
{
	public partial class PropertyGetTests
	{
		[TestMethod]
		public void IntPropertyToThrowTest()
		{
			var exception = new Exception();

			_overmock.Mock(t => t.Int)
				.ToThrow(exception);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			try
			{
				var model = target.Int;

				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(exception, ex);
			}
		}

		[TestMethod]
		public void StringPropertyToThrowTest()
		{
			var exception = new Exception();

			_overmock.Mock(t => t.String)
				.ToThrow(exception);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			try
			{
				var model = target.String;

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

			_overmock.Mock(t => t.Model)
				.ToThrow(exception);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			try
			{
				var model = target.Model;

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

			_overmock.Mock(t => t.ListOfModels)
				.ToThrow(exception);

			var target = _overmock.Target;

			Assert.IsNotNull(target);

			try
			{
				var model = target.ListOfModels;

				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(exception, ex);
			}
		}
	}
}
