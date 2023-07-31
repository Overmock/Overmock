using Overmock.Examples.Storage;
using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
{
    [TestClass]
	public class MethodGenericNoParametersTests
	{
		private IOvermock<IGenericMethodsTestInterface> _genericMethodsTestInterface = null!;

		[TestInitialize]
		public void Initialize()
		{
			_genericMethodsTestInterface = Overmocked.Interface<IGenericMethodsTestInterface>();
		}

		[TestMethod]
		public void TestWithGenericParametersOnMethod()
		{
			var expected = new List<UserStory> { new  UserStory() };

			_genericMethodsTestInterface.Override(m => m.MethodWithNoParamsAndReturnsEnumerableOfT<UserStory>())
				.ToReturn(expected);

			var target = _genericMethodsTestInterface.Target;

			var actual = target.MethodWithNoParamsAndReturnsEnumerableOfT<UserStory>();
			Assert.IsTrue(true);
			Assert.IsNotNull(target);
			CollectionAssert.AreEqual(expected, actual.ToList());
		}
	}
}
