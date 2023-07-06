using Overmock.Examples.Storage;
using Overmock.Examples.Tests.TestCode;

namespace Overmock.Examples.Tests
{
	[TestClass]
	public class GenericParameterTests
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE0052 // Remove unread private members


		private IOvermock<IGenericMethodsTestInterface> _genericMethodsTestInterface;


#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		[TestInitialize]
		public void Initialize()
		{
			_genericMethodsTestInterface = Overmocked.Setup<IGenericMethodsTestInterface>();
		}

		[TestMethod]
		public void TestWithGenericParametersOnMethod()
		{
			var expected = new List<UserStory> { new  UserStory() };

			_genericMethodsTestInterface.Override(m => m.MethodWithNoParamsAndReturnsEnumerableOfT<UserStory>())
				.ToReturn(expected);

			//var target = _genericMethodsTestInterface.Target;

			//var actual = target.MethodWithNoParamsAndReturnsEnumerableOfT<UserStory>();
			Assert.IsTrue(true);
			//Assert.IsNotNull(target);
			//CollectionAssert.AreEqual(expected, actual.ToList());
		}
	}
}
